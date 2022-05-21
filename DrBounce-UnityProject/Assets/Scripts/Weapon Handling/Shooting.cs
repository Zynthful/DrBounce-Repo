using UnityEngine;
using UnityEngine.Events;
using MoreMountains.NiceVibrations;
using SamDriver.Decal;
using System.Collections.Generic;

public class Shooting : MonoBehaviour
{
    public enum GunModes
    {
        Explosives,
    }

    [Header("Declarations")]
    [SerializeField]
    private Gun shooter = null;
    [SerializeField]
    private Health health = null;
    [SerializeField]
    private GunThrowing gunThrowing = null;
    [SerializeField]
    private GameObject impactEffect;
    [SerializeField]
    private ParticleSystem chargedShotPS;
    [SerializeField]
    private Material bulletDecalMaterial;
    [SerializeField]
    private Camera fpsCam;
    [SerializeField]
    private Animator anim;

    private ObjectPooler pool;
    private DecalManager decalM;

    [Header("Damage")]
    private int damage = 0;     //current damage value

    [Header("Charges")]
    private int gunCharge = 0;    //amount of times the gun has been bounced successfully
    private int maxCharges = 99;

    private bool hasCharge = false;
    public bool GetHasCharge() { return hasCharge; }

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

    private bool maxDamage;
    private bool canShoot;

    #region Events

    #region UnityEvents
    [Header("Gun Charge Events")]
    public UnityEvent<int> onChargeUpdate;
    public UnityEvent onChargesEmpty;
    public UnityEvent<bool> onHasCharge;
    public UnityEvent<bool> onHasChargeAndIsHeld;
    public UnityEvent onFirstGainChargeSinceEmpty;
    public UnityEvent onAtFullCharge;
    public UnityEvent<bool> onIsAtFullCharge;

    [Header("Firing Events")]
    public UnityEvent onFire;
    public UnityEvent onUnchargedShotFired;
    public UnityEvent<int> onChargedShotFired;
    public UnityEvent onSingleChargeShotFired;
    public UnityEvent onExplosiveShot;

    [Header("Max Charge Shot Events")]
    public UnityEvent<float> onChargingMaxShotProgress;     // Every frame that the max shot is charging. Passes progress as a percentage between 0-1.
    public UnityEvent onChargeMaxShotBegin;                 // When starting to charge the max shot by holding the button
    public UnityEvent onChargeMaxShotCancel;                // When cancelling the max shot charge by letting go of the button too early
    public UnityEvent onMaxShotCharged;                     // When the max shot has been fully charged (but not fired)
    public UnityEvent<int> onMaxShotFired;                  // When the max shot has been fired. Passes number of charges consumed.

    [Header("Heal Fail Events")]
    public UnityEvent onFailHeal;
    public UnityEvent onFailHealFullHP;
    public UnityEvent onFailHealNoCharge;
    public UnityEvent onFailHealNotHeld;
    public UnityEvent onFailHealAlreadyDead;

    [Header("Mouse Look Events")]
    public UnityEvent<bool> onEnemyHover;
    #endregion

    #region GameEvents
    [Header("Game Events")]
    [Tooltip("Passes gun charge")]
    public GameEventInt _onChargeUpdate = null;
    public GameEvent _onUnchargedShotFired = null;
    [Tooltip("Passes gun charge")]
    public GameEventInt _onChargedShotFired = null;
    public GameEvent _onChargesEmpty = null;
    [Tooltip("Occurs on charge update. Passes whether the gun has charge or not")]
    public GameEventBool _onHasCharge = null;
    [Tooltip("Occurs on charge update. Passes whether the gun has charge and is currently being held or not")]
    public GameEventBool _onHasChargeAndIsHeld = null;
    public GameEvent _onFirstGainChargeSinceEmpty = null;
    [Tooltip("Passes whether the player is hovering over an enemy")]
    public GameEventBool _onEnemyHover = null;
    public GameEvent _onExplosiveShot = null;
    #endregion

    #endregion

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

        if (value >= maxCharges)
        {
            if (gunCharge != maxCharges)
                onAtFullCharge.Invoke();

            gunCharge = maxCharges;
        }
        else
        {
            gunCharge = value;
        }

        onIsAtFullCharge.Invoke(gunCharge == maxCharges);

        anim.SetInteger("ChargesLeft", gunCharge);

        onChargeUpdate?.Invoke(gunCharge);
        _onChargeUpdate?.Raise(gunCharge);
        
        hasCharge = HasCharge();
    }

    private bool HasCharge()
    {
        /*
        // Only execute the rest of the method if hasCharge would change
        bool _hasCharge = gunCharge >= 1;
        if (hasCharge == _hasCharge)
            return hasCharge;
        */

        bool _hasCharge = gunCharge >= 1;

        onHasCharge?.Invoke(_hasCharge);
        _onHasCharge?.Raise(_hasCharge);
        onHasChargeAndIsHeld?.Invoke(_hasCharge && gunThrowing.GetIsHeld());
        _onHasChargeAndIsHeld?.Raise(_hasCharge && gunThrowing.GetIsHeld());

        if (!_hasCharge)
        {
            onChargesEmpty?.Invoke();
            _onChargesEmpty?.Raise();
            chargedShotPS.Clear();
        }

        return _hasCharge;
    }

    private void CheckForHoverOverEnemy() 
    {
        RaycastHit Reticleinfo;
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out Reticleinfo, shooter.normalRange))
        {
            Enemy enemy = Reticleinfo.transform.GetComponent<Enemy>();
            onEnemyHover?.Invoke(enemy != null);
            _onEnemyHover?.Raise(enemy != null);
        }
    }

    private void ShootStarted()
    {

    }

    private void ShootReleased()
    {
        if (GameManager.s_Instance.paused)
            return;

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

    public void CancelMaxShot()
    {
        maxShotCharging = false;
        onChargingMaxShotProgress?.Invoke(0.0f);
        maxShotCharged = false;
        onChargeMaxShotCancel?.Invoke();
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

        onFire.Invoke();

        // Fire a single charged shot if we have sufficient charges
        if (gunCharge > 0)
        {
            HandleChargedShot(1);
            onSingleChargeShotFired?.Invoke();
        }

        // Fire an uncharged shot
        else if(canShoot)
        {
            // Trigger used unlock for the first time, if it's the first time we've done so
            if (UnlockTracker.instance.lastUnlock == UnlockTracker.UnlockTypes.NormalShooting && !UnlockTracker.instance.usedUnlock)
            {
                UnlockTracker.instance.UsedUnlockFirstTime();
            }

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
        //ChargedFeedback?.PlayFeedbacks();
        
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
        //ChargedFeedback?.StopFeedbacks();
        chargedShotPS.Clear();
    }

    public void Catch()
    {
        //ChargedFeedback?.PlayFeedbacks();
    }

    public void Reset()
    {
        if (transform.parent == null)
        {
            //ChargedFeedback?.StopFeedbacks();
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
                if (!health.GetIsAtFullHealth())
                {
                    // Successful heal
                    if (!health.GetIsDead())
                    {
                        int healAmount = GraphCalculator(shooter.healGraph, gunCharge);
                        health.Heal(healAmount);

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
                    // Fail heal: Already dead
                    else
                    {
                        onFailHeal?.Invoke();
                        onFailHealAlreadyDead?.Invoke();
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
