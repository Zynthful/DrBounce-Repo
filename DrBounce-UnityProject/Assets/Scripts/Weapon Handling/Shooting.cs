﻿using UnityEngine;
using UnityEngine.Events;
using MoreMountains.Feedbacks;
using MoreMountains.NiceVibrations;
using SamDriver.Decal;

public class Shooting : MonoBehaviour
{
    public enum GunModes
    {
        Explosives,
    }

    private DecalManager decalM;

    [SerializeField]
    private Gun shooter = null;
    [SerializeField]
    private Health health = null;

    private ObjectPooler pool;
    public InputMaster controls;
    public Camera fpsCam;
    public Animator anim;

    [Header("Damage")]
    private int damage = 0;     //current damage value

    [Header("Charges")]
    private int gunCharge = 0;    //amount of times the gun has been bounced successfully
    private bool hasCharge = false;

    [Header("Fire Rate")]
    private float timeSinceLastShot = 0;

    [SerializeField]
    [Tooltip("Duration in seconds that the shoot button must be held in order to fully charge a max charge shot.")]
    private float holdTimeToFullCharge = 1.0f;
    [SerializeField]
    [Tooltip("Duration in seconds that the shoot button must be held before cancelling does NOT trigger a regular shot.")]
    private float holdTimeThreshold = 0.3f;

    private float currentHoldTime = 0.0f;
    private bool holdingShoot = false;
    private bool maxShotCharging = false;
    private bool maxShotCharged = false;

    #region Events

    #region UnityEvents
    [Header("Unity Events")]
    [SerializeField]
    private UnityEvent<int> onChargeUpdate = null;
    [SerializeField]
    private UnityEvent onUnchargedShotFired = null;
    [SerializeField]
    private UnityEvent<int> onChargedShotFired = null;
    [SerializeField]
    private UnityEvent onSingleChargeShotFired = null;
    [SerializeField]
    private UnityEvent onChargesEmpty = null;
    [SerializeField]
    private UnityEvent<bool> onHasCharge = null;
    public UnityEvent<bool> onHasChargeAndIsHeld = null;
    [SerializeField]
    private UnityEvent onFirstGainChargeSinceEmpty = null;
    [SerializeField]
    private UnityEvent<bool> onEnemyHover = null;
    [SerializeField]
    private UnityEvent onExplosiveShot = null;

    [Header("Max Charge Shot Unity Events")]
    [SerializeField]
    private UnityEvent<float> onChargingMaxShotProgress = null;     // Every frame that the max shot is charging. Passes progress as a percentage between 0-1.
    [SerializeField]
    private UnityEvent onChargeMaxShotBegin = null;                 // When starting to charge the max shot by holding the button
    [SerializeField]
    private UnityEvent onChargeMaxShotCancel = null;                // When cancelling the max shot charge by letting go of the button too early
    [SerializeField]
    private UnityEvent onMaxShotCharged = null;                     // When the max shot has been fully charged (but not fired)
    [SerializeField]
    private UnityEvent<int> onMaxShotFired = null;                  // When the max shot has been fired. Passes number of charges consumed.
    #endregion

    #region GameEvents
    [Header("Game Events")]
    [SerializeField]
    [Tooltip("Passes gun charge")]
    private GameEventInt _onChargeUpdate = null;
    [SerializeField]
    private GameEvent _onUnchargedShotFired = null;
    [SerializeField]
    [Tooltip("Passes gun charge")]
    private GameEventInt _onChargedShotFired = null;
    [SerializeField]
    private GameEvent _onChargesEmpty = null;
    [SerializeField]
    [Tooltip("Occurs on charge update. Passes whether the gun has charge or not")]
    private GameEventBool _onHasCharge = null;
    [Tooltip("Occurs on charge update. Passes whether the gun has charge and is currently being held or not")]
    public GameEventBool _onHasChargeAndIsHeld = null;
    [SerializeField]
    private GameEvent _onFirstGainChargeSinceEmpty = null;
    [SerializeField]
    [Tooltip("Passes whether the player is hovering over an enemy")]
    private GameEventBool _onEnemyHover = null;
    [SerializeField]
    private GameEvent _onExplosiveShot = null;
    #endregion

    #endregion

    public delegate void Activated(int value);
    public static event Activated OnActivated;

    public MMFeedbacks ChargedFeedback;
    public GameObject impactEffect;

    [SerializeField] ParticleSystem chargedShotPS;
    [SerializeField] Material bulletDecalMaterial;

    private bool maxDamage;

    // Start is called before the first frame update
    private void Start()
    {
        // Initialise charge
        SetCharge(gunCharge);

        pool = ObjectPooler.Instance;
        decalM = DecalManager.Instance;
        CheckForHoverOverEnemy();
    }

    // Update is called once per frame
    private void Update()
    {
        timeSinceLastShot += Time.deltaTime;

        CheckForHoverOverEnemy();

        if (shooter.canRepeatShoot && holdingShoot)
        {
            Shoot();
        }

        // Handle max shot charging timer
        if (maxShotCharging)
        {
            currentHoldTime += Time.deltaTime;
            onChargingMaxShotProgress.Invoke(currentHoldTime / holdTimeToFullCharge);
        }

        if (currentHoldTime > holdTimeToFullCharge && !maxShotCharged)
        {
            print("Max Charge!");
            maxShotCharged = true;
            //maxDamage keeps track of a held charge shot for damage calculations
            maxDamage = true;
            onMaxShotCharged?.Invoke();
        }
    }

    private void Awake()
    {
        controls = InputManager.inputMaster;
    }

    private void OnEnable()
    {
        controls.Player.Shoot.started += _ => TryStartCharging();
        controls.Player.Shoot.canceled += _ => ReleaseCharge();

        controls.Player.RecallGun.performed += _ => Reset();
        controls.Player.Healing.performed += _ => Healing();

        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Player.Shoot.started -= _ => TryStartCharging();
        controls.Player.Shoot.canceled -= _ => ReleaseCharge();

        controls.Player.RecallGun.performed -= _ => Reset();
        controls.Player.Healing.performed -= _ => Healing();

        controls.Disable();
    }

    private void TryStartCharging() 
    {
        holdingShoot = true;

        if (!GameManager.s_Instance.paused && transform.parent != null)
        {
            // Only begin charging max charge shot if we have charge
            if (gunCharge > 0)
            {
                onChargeMaxShotBegin?.Invoke();
                maxShotCharging = true;
            }
        }
    }

    private void ReleaseCharge()
    {
        holdingShoot = false;

        if (!GameManager.s_Instance.paused && transform.parent != null && maxShotCharging)
        {
            maxShotCharging = false;
            // Release a max charged shot if we've fully charged
            if (maxShotCharged)
            {
                maxShotCharged = false;
                HandleChargedShot(gunCharge);
            }
            // Cancel into a regular shot if we haven't reached the threshold
            else if (currentHoldTime < holdTimeThreshold)
            {
                onChargeMaxShotCancel?.Invoke();
                Shoot();
            }
            // Cancel without firing a regular shot
            else
            {
                onChargeMaxShotCancel?.Invoke();
            }
            currentHoldTime = 0.0f;
        }
    }

    /// <summary>
    /// Sets gunCharge equal to the input value
    /// Use this whenever updating gunCharge, so it raises onChargeUpdate with it
    /// </summary>
    /// <param name="value"></param>
    public void SetCharge(int value)
    {
        // Is the gun gaining charge for the first time since emptied?
        if (gunCharge <= 0 && value >= 1)
        {
            onFirstGainChargeSinceEmpty?.Invoke();
            _onFirstGainChargeSinceEmpty?.Raise();
        }

        gunCharge = value;

        onChargeUpdate?.Invoke(gunCharge);
        _onChargeUpdate?.Raise(gunCharge);

        CheckIfCharged();
    }

    public void CheckIfCharged()
    {
        if (gunCharge >= 1)
        {
            hasCharge = true;

            ChargedFeedback?.PlayFeedbacks();
            // this will need to be rewritten eventually?
            if (transform.parent)
            {
                onHasChargeAndIsHeld?.Invoke(true);
                _onHasChargeAndIsHeld?.Raise(true);
            }
            else
            {
                onHasChargeAndIsHeld?.Invoke(false);
                _onHasChargeAndIsHeld?.Raise(false);
            }
        }
        else
        {
            hasCharge = false;

            onHasChargeAndIsHeld?.Invoke(false);
            _onHasChargeAndIsHeld?.Raise(false);
            onChargesEmpty?.Invoke();
            _onChargesEmpty?.Raise();

            ChargedFeedback?.StopFeedbacks();
            chargedShotPS.Clear();
        }

        onHasCharge?.Invoke(hasCharge);
        _onHasCharge?.Raise(hasCharge);
        anim.SetInteger("ChargesLeft", gunCharge);
    }

    private void CheckForHoverOverEnemy() 
    {
        RaycastHit Reticleinfo;
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out Reticleinfo, shooter.normalRange))
        {
            Enemy enemy = Reticleinfo.transform.GetComponent<Enemy>();
            if (enemy != null)  //no charge, and over an enemy
            {
                onEnemyHover?.Invoke(true);
                _onEnemyHover?.Raise(true);
                //print(enemy.transform.name + " is being hovered over!");
            }
            else if (enemy != null && gunCharge > 0)   //has charge, and is over an enemy
            {
                //funny feedback time
            }
            else   //could or could not have charge, and is NOT over an enemy
            {
                onEnemyHover?.Invoke(false);
                _onEnemyHover?.Raise(false);
            }
        }
    }

    private void Shoot() 
    {
        // Checks:
        //  - Is not paused
        //  - Object has a parent
        //  - Is not already shooting
        if (!GameManager.s_Instance.paused && transform.parent != null && timeSinceLastShot > shooter.fireRate && !maxShotCharging)  
        {
            timeSinceLastShot = 0;

            // Are we trying to fire a single charged shot?
            if (gunCharge > 0)
            {
                HandleChargedShot(1);
            }

            // Is it an uncharged/basic shot?
            else
            {
                onUnchargedShotFired?.Invoke();
                _onUnchargedShotFired?.Raise();

                //ChargedFeedback?.StopFeedbacks();
                damage = Mathf.RoundToInt(shooter.damageGraph[0].y);

                RaycastHit Hitinfo;
                if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out Hitinfo, shooter.normalRange))
                {
                    Hitinfo.transform.GetComponent<Spin>()?.OnStart(Vector3.Magnitude(Hitinfo.normal));
                    Instantiate(impactEffect, Hitinfo.point, Quaternion.LookRotation(Hitinfo.normal));

                    //print(Hitinfo.transform.name + " hit!");
                    EnemyHealth enemy = Hitinfo.transform.GetComponent<EnemyHealth>();
                    if (enemy != null)
                    {
                        enemy.Damage(damage);
                    }
                    else
                    {
                        decalM.SpawnDecal(Hitinfo.point, Hitinfo.normal, 0.4f, bulletDecalMaterial);
                    }
                }
            }

            //Instantiate(bullet, transform.position, transform.rotation, null); Change to use raycast

            //Debug.LogWarning("BANG!!!");
            //Debug.LogWarning("You shot for " + damage);
        }
    }

    private void HandleChargedShot(int chargeUsed)
    {
        if (chargeUsed == gunCharge)
        {
            onMaxShotFired?.Invoke(chargeUsed);
        }
        else if (chargeUsed == 1)
        {
            onSingleChargeShotFired?.Invoke();
        }

        ChargedFeedback?.PlayFeedbacks();
        
        switch(shooter.chargeShot)
        {
            case GunModes.Explosives:
                onExplosiveShot?.Invoke();
                _onExplosiveShot?.Raise();

                if (maxDamage) //if charge shot has been held down
                {
                    //Do standard damage multiplied by the number of charges
                    shooter.chargeBullet.damage = GraphCalculator(shooter.damageGraph, gunCharge) * gunCharge;
                    maxDamage = false;
                }

                //If charge shot hasn't been held down (consecutive charge shot)
                else
                {
                    //do standard damage for 1 charge - currently 100
                    shooter.chargeBullet.damage = GraphCalculator(shooter.damageGraph, 1);
                }

                GameObject obj = pool.SpawnBulletFromPool("ExplosiveShot", (PlayerMovement.player.position + (Vector3.up * (PlayerMovement.player.localScale.y / 8f))) + (fpsCam.transform.TransformDirection(Vector3.forward).normalized * 2.5f), Quaternion.Euler(fpsCam.transform.TransformDirection(Vector3.forward)), fpsCam.transform.TransformDirection(Vector3.forward).normalized, shooter.chargeBullet, null);
                obj.GetComponentInChildren<ExplosiveShot>().comboSize = chargeUsed;
                break;
        }

        onChargedShotFired?.Invoke(chargeUsed);
        _onChargedShotFired?.Raise(chargeUsed);

        if (shooter.useAllChargesOnUse)
        {
            SetCharge(0);
        }
        else
        {
            SetCharge(gunCharge - chargeUsed);   // Minus 1 from gunCharge
        }
    }

    public void Bounce(int bounceCount) 
    {
        SetCharge(bounceCount);
        ChargedFeedback?.StopFeedbacks();
        chargedShotPS.Clear();
    }

    public void Catch()
    {
        ChargedFeedback?.PlayFeedbacks();
    }

    public void Reset()
    {
        ChargedFeedback?.StopFeedbacks();
        chargedShotPS.Clear();
        if (anim != null) anim.SetInteger("ChargesLeft", gunCharge);
        SetCharge(0);
    }

    public void Dropped() 
    {
        if (!transform.parent)  //if the gun is dropped and has no parent
        {
            Reset();
        }
    }

    private int GraphCalculator(Vector2[] graph, int charges) //🐌
    {
        foreach (Vector2 amount in graph)  //loops through the vector 2 (graph)
        {
            if (amount.x == charges)
            {
                return Mathf.RoundToInt(amount.y);
            }
        }
        if (charges >= graph.Length) //in case you over the max
        {
            return Mathf.RoundToInt(graph[graph.Length - 1].y);
        }
        return 0; // you lose you get nothing
    }

    private void Healing() 
    {
        if (gunCharge > 0 && !health.GetIsAtFullHealth()) 
        {
            int healAmount = GraphCalculator(shooter.healGraph, gunCharge);
            OnActivated?.Invoke(healAmount);    //calls the player heal function

            if (shooter.useAllChargesOnUse)
            {
                SetCharge(0);
            }
            else
            {
                SetCharge(gunCharge - 1);   // Minus 1 from gunCharge
            }

            if (gunCharge == 0)
            {
                Reset();
            }
        }
    }
}
