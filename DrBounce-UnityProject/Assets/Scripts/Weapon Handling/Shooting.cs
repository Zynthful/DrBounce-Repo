﻿using UnityEngine;
using UnityEngine.Events;
using MoreMountains.Feedbacks;
using MoreMountains.NiceVibrations;
using SamDriver.Decal;
using System.Collections.Generic;

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
    [SerializeField]
    private GunThrowing gunThrowing = null;

    private ObjectPooler pool;
    public Camera fpsCam;
    public Animator anim;

    [Header("Damage")]
    private int damage = 0;     //current damage value

    [Header("Charges")]
    private int gunCharge = 0;    //amount of times the gun has been bounced successfully
    private bool hasCharge = false;
    private int maxCharges = 99;

    [Header("Fire Rate")]
    private float timeSinceLastShot = 0;

    [Header("Max Shot Settings")]

    private float holdTimeToFullCharge = 0;
    private float chargeCancelThreshold = 0;
    private float chargeBeginThreshold = 0;
    private int minChargeToMaxShot = 0;

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
    public UnityEvent<float> onChargingMaxShotProgress = null;     // Every frame that the max shot is charging. Passes progress as a percentage between 0-1.
    public UnityEvent onChargeMaxShotBegin = null;                 // When starting to charge the max shot by holding the button
    public UnityEvent onChargeMaxShotCancel = null;                // When cancelling the max shot charge by letting go of the button too early
    public UnityEvent onMaxShotCharged = null;                     // When the max shot has been fully charged (but not fired)
    public UnityEvent<int> onMaxShotFired = null;                  // When the max shot has been fired. Passes number of charges consumed.

    [Header("Heal Fail Events")]
    public UnityEvent onFailHeal = null;
    public UnityEvent onFailHealFullHP = null;
    public UnityEvent onFailHealNoCharge = null;
    public UnityEvent onFailHealNotHeld = null;
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

    private bool canShoot;

    // Start is called before the first frame update
    private void Start()
    {
        // Updates max charges to a given value (more than 0)
        UpdateMaxCharges(shooter.maxCharges);

        // Initialise charge
        SetCharge(gunCharge);

        pool = ObjectPooler.Instance;
        decalM = DecalManager.Instance;
        CheckForHoverOverEnemy();

        holdTimeToFullCharge = shooter.holdTimeToFullCharge;
        chargeCancelThreshold = shooter.chargeCancelThreshold; 
        chargeBeginThreshold = shooter.chargeBeginThreshold; 
        minChargeToMaxShot = shooter.minChargeToMaxShot;
    }

    // Update is called once per frame
    private void Update()
    {
        holdingShoot = InputManager.inputMaster.Player.Shoot.ReadValue<float>() >= 0.2f;

        if (GameManager.s_Instance.paused)
            return;

        timeSinceLastShot += Time.deltaTime;

        CheckForHoverOverEnemy();

        // Handle max shot charging
        if (gunThrowing.GetIsHeld() && holdingShoot)
        {
            currentHoldTime += Time.deltaTime;

            if (gunCharge > minChargeToMaxShot)
            {
                // Begin charging max shot once we've passed the threshold and have sufficient charge
                if (!maxShotCharged && !maxShotCharging && (currentHoldTime / holdTimeToFullCharge) >= chargeBeginThreshold)
                {
                    onChargeMaxShotBegin?.Invoke();
                    maxShotCharging = true;
                }

                // Charge max shot progress
                if (!maxShotCharged && maxShotCharging)
                {
                    onChargingMaxShotProgress.Invoke(currentHoldTime / holdTimeToFullCharge);

                    // If we've fully charged our max shot
                    if (currentHoldTime > holdTimeToFullCharge)
                    {
                        maxShotCharged = true;
                        maxShotCharging = false;

                        //maxDamage keeps track of a held charge shot for damage calculations
                        maxDamage = true;

                        onMaxShotCharged?.Invoke();
                    }
                }
            }
            else if (shooter.canRepeatShoot)
            {
                TryShoot();
            }
        }
    }

    private void Awake()
    {
        GunThrowing throwing = GetComponent<GunThrowing>();
        if (gunThrowing == null && throwing != null)
        {
            gunThrowing = throwing;
        }
    }

    private void OnEnable()
    {
        InputManager.inputMaster.Player.Shoot.started += _ => ShootStarted();
        InputManager.inputMaster.Player.Shoot.canceled += _ => ShootReleased();

        InputManager.inputMaster.Player.Recall.performed += _ => Reset();
        InputManager.inputMaster.Player.Heal.performed += _ => TryHeal();

        gunThrowing.onThrown.AddListener(ShootReleased);
    }

    private void OnDisable()
    {
        InputManager.inputMaster.Player.Shoot.started -= _ => ShootStarted();
        InputManager.inputMaster.Player.Shoot.canceled -= _ => ShootReleased();

        InputManager.inputMaster.Player.Recall.performed -= _ => Reset();
        InputManager.inputMaster.Player.Heal.performed -= _ => TryHeal();

        gunThrowing.onThrown.RemoveListener(ShootReleased);
    }

    public void UpdateCanShoot(bool value)
    {
        canShoot = value;
    }

    public void UpdateMaxCharges(int value)
    {
        if(value > 0)
            maxCharges = value;
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

        if (value > maxCharges)
            gunCharge = maxCharges;
        else
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

    private void ShootStarted()
    {

    }

    private void ShootReleased()
    {
        if (!GameManager.s_Instance.paused)
        {
            maxShotCharging = false;
            onChargingMaxShotProgress?.Invoke(0.0f);

            // Release a max charged shot if we've fully charged and we're holding the gun
            if (maxShotCharged && gunThrowing.GetIsHeld())
            {
                maxShotCharged = false;
                HandleChargedShot(gunCharge);
                onMaxShotFired?.Invoke(gunCharge);
            }

            // Cancel if we haven't reached the threshold, shooting if we're still holding the gun
            else if (currentHoldTime / holdTimeToFullCharge < chargeCancelThreshold)
            {
                onChargeMaxShotCancel?.Invoke();

                if (gunThrowing.GetIsHeld())
                {
                    TryShoot();
                }
            }

            // Cancel without trying to fire a regular shot
            else
            {
                onChargeMaxShotCancel?.Invoke();
            }

            currentHoldTime = 0.0f;
        }
    }

    /// <summary>
    /// Attempts to shoot, checking if we're unpaused, holding the gun, the gun is not cooling down, and we're not already charging a max shot
    /// </summary>
    private void TryShoot()
    {
        if (!GameManager.s_Instance.paused && gunThrowing.GetIsHeld() && !IsCoolingDown() && !maxShotCharging)
        {
            Shoot();
        }
    }

    private void Shoot() 
    {
        timeSinceLastShot = 0;

        // Fire a single charged shot if we have sufficient charges
        if (gunCharge > 0)
        {
            HandleChargedShot(1);
            onSingleChargeShotFired?.Invoke();
        }

        // Fire an uncharged shot
        else if(canShoot)
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
                Stun enemyStun = Hitinfo.transform.GetComponent<Stun>();
                if (enemy != null)
                {
                    enemy.Damage(damage);
                    enemyStun.Hit();
                }
                else
                {
                    decalM.SpawnDecal(Hitinfo.point, Hitinfo.normal, 0.4f, Hitinfo.transform, DecalManager.DecalType.bullet);
                }
            }
        }
    }

    private void HandleChargedShot(int chargeUsed)
    {
        ChargedFeedback?.PlayFeedbacks();
        
        switch(shooter.chargeShot)
        {
            case GunModes.Explosives:
                onExplosiveShot?.Invoke();
                _onExplosiveShot?.Raise();

                if (maxDamage) //if charge shot has been held down
                {
                    //Do standard damage multiplied by the number of charges
                    shooter.chargeBullet.damage = GraphCalculator(shooter.damageGraph, gunCharge);
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
        if (transform.parent == null)
        {
            ChargedFeedback?.StopFeedbacks();
            chargedShotPS.Clear();
            if (anim != null) anim.SetInteger("ChargesLeft", gunCharge);
            SetCharge(0);
        }
    }

    public void Dropped() 
    {
        if (!gunThrowing.GetIsHeld()) 
        {
            Reset();
        }
    }

    public int GetCharges()
    {
        return gunCharge;
    }

    private int GraphCalculator(List<Vector2> graph, int charges) //🐌
    {
        foreach (Vector2 amount in graph)  //loops through the vector 2 (graph)
        {
            if (amount.x == charges)
            {
                return Mathf.RoundToInt(amount.y);
            }
        }
        if (charges >= graph.Count) //in case you over the max
        {
            return Mathf.RoundToInt(graph[graph.Count - 1].y);
        }
        return 0; // you lose you get nothing
    }

    private void TryHeal() 
    {
        if (gunThrowing.GetIsHeld()) 
        {
            if (gunCharge > 0)
            {
                // Successful heal
                if (!health.GetIsAtFullHealth())
                {
                    int healAmount = GraphCalculator(shooter.healGraph, gunCharge);
                    OnActivated?.Invoke(healAmount);    //calls the player heal function

                    if (shooter.useAllChargesOnUse)
                    {
                        SetCharge(0);
                        Reset();
                    }
                    else
                    {
                        SetCharge(gunCharge - 1);   // Minus 1 from gunCharge

                        if (gunCharge <= 0)
                        {
                            Reset();
                        }
                    }
                }
                // Fail heal: Full HP
                else
                {
                    onFailHeal?.Invoke();
                    onFailHealFullHP?.Invoke();
                }
            }
            // Fail heal: No Charge
            else
            {
                onFailHeal?.Invoke();
                onFailHealNoCharge?.Invoke();
            }
        }
        // Fail heal: Not held
        else
        {
            onFailHeal?.Invoke();
            onFailHealNotHeld?.Invoke();
        }
    }

    private bool IsCoolingDown()
    {
        return timeSinceLastShot <= shooter.fireRate;
    }
}
