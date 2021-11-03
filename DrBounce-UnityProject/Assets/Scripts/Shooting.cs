using UnityEngine;
using MoreMountains.Feedbacks;
using MoreMountains.NiceVibrations;

public class Shooting : MonoBehaviour
{
    public enum GunModes
    {
        Explosives,
    }

    [SerializeField] private Gun shooter = null;
    private ObjectPooler pool;
    public InputMaster controls;
    public Camera fpsCam;
    public Animator anim;

    private bool repeatedShooting = false;
    [Header("Damage")]
    [HideInInspector]
    public int damage = 10;     //current damage value

    [Header("Charges")]
    private int gunCharge = 0;    //amount of times the gun has been bounced successfully
    private bool hasCharge = false;

    [Header("Fire Rate")]
    private float timeSinceLastShot = 0;

    #region Events
    [Header("Events")]

    [SerializeField]
    [Tooltip("Passes gun charge")]
    private GameEventInt onChargeUpdate = null;

    [SerializeField]
    private GameEvent onUnchargedShotFired = null;

    [SerializeField]
    [Tooltip("Passes gun charge")]
    private GameEventInt onChargedShotFired = null;

    [SerializeField]
    private GameEvent onChargesEmpty = null;

    [SerializeField]
    [Tooltip("Occurs on charge update. Passes whether the gun has charge or not")]
    private GameEventBool onHasCharge = null;

    [Tooltip("Occurs on charge update. Passes whether the gun has charge and is currently being held or not")]
    public GameEventBool onHasChargeAndIsHeld = null;

    [SerializeField]
    private GameEvent onFirstGainChargeSinceEmpty = null;

    [SerializeField]
    [Tooltip("Passes whether the player is hovering over an enemy")]
    private GameEventBool onEnemyHover = null;

    public delegate void Activated(int value);
    public static event Activated OnActivated;

    #endregion

    [Header("Feedbacks")]
    public MMFeedbacks BasicShootFeedback;
    public MMFeedbacks ChargedFeedback;
    public MMFeedbacks FirstChargedShotFeedback;
    [SerializeField] ParticleSystem chargedShotPS;

    // Start is called before the first frame update
    void Start()
    {
        // Initialise charge
        SetCharge(gunCharge);

        pool = ObjectPooler.Instance;
        CheckForHoverOverEnemy();
    }

    // Update is called once per frame
    void Update()
    {
        timeSinceLastShot += Time.deltaTime;

        CheckForHoverOverEnemy();

        if (repeatedShooting) Shoot();
    }

    private void Awake()
    {
        controls = new InputMaster();
        controls.Player.Shoot.performed += _ => Shoot();
        controls.Player.Shoot.canceled += _ => StopShooting();
        controls.Player.RecallGun.performed += _ => Reset();
        controls.Player.Healing.performed += _ => Healing();
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }

    // Use this to update chargesLeft so it raises the onChargeUpdate event along with it

    /// <summary>
    /// Sets gunCharge equal to the input value
    /// Use this whenever update gunCharge, so it raises onChargeUpdate with it
    /// </summary>
    /// <param name="value"></param>
    public void SetCharge(int value)
    {
        // Is the gun gaining charge for the first time since emptied?
        if (gunCharge <= 0 && value >= 1)
        {
            onFirstGainChargeSinceEmpty?.Raise();
        }

        gunCharge = value;

        onChargeUpdate?.Raise(gunCharge);

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
                onHasChargeAndIsHeld?.Raise(true);
            }
            else
            {
                onHasChargeAndIsHeld?.Raise(false);
            }
        }
        else
        {
            hasCharge = false;

            onHasChargeAndIsHeld?.Raise(false);
            onChargesEmpty?.Raise();

            ChargedFeedback?.StopFeedbacks();
            chargedShotPS.Clear();
        }

        onHasCharge?.Raise(hasCharge);
        anim.SetInteger("ChargesLeft", gunCharge);
    }

    private void CheckForHoverOverEnemy() 
    {
        RaycastHit Reticleinfo;
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out Reticleinfo, shooter.normalRange))
        {
            Enemy enemy = Reticleinfo.transform.GetComponent<Enemy>();
            if (enemy != null)
            {
                onEnemyHover?.Raise(true);
                //print(enemy.transform.name + " is being hovered over!");
            }
            else
            {
                onEnemyHover?.Raise(false);
            }
        }
    }

    private void Shoot() 
    {
        // Checks:
        //  - Is not paused
        //  - Object has a parent
        //  - Is not already shooting
        if (!GameManager.s_Instance.paused && transform.parent != null && timeSinceLastShot > shooter.fireRate)  
        {
            if (shooter.canRepeatShoot) repeatedShooting = true;

            timeSinceLastShot = 0;

            if(gunCharge > 0) HandleComboShot();

            // Is it an uncharged/basic shot?
            else if (gunCharge <= 0)
            {
                onUnchargedShotFired?.Raise();

                BasicShootFeedback?.PlayFeedbacks();

                //ChargedFeedback?.StopFeedbacks();
                damage = Mathf.RoundToInt(shooter.damageGraph[0].y);

                RaycastHit Hitinfo;
                if(Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out Hitinfo, shooter.normalRange))
                {
                    //print(Hitinfo.transform.name + " hit!");
                    Enemy enemy = Hitinfo.transform.GetComponent<Enemy>();
                    if(enemy != null)
                    {
                        enemy.TakeDamage(damage);
                    }
                }
            }

            //Instantiate(bullet, transform.position, transform.rotation, null); Change to use raycast

            //Debug.LogWarning("BANG!!!");
            //Debug.LogWarning("You shot for " + damage);
        }
    }

    private void HandleComboShot()
    {
    	ChargedFeedback?.PlayFeedbacks();	

        switch(shooter.chargeShot)
        {
            case GunModes.Explosives:
                FirstChargedShotFeedback?.PlayFeedbacks();

                shooter.chargeBullet.damage = DamageAmountCalc(gunCharge);

                GameObject obj = pool.SpawnBulletFromPool("ExplosiveShot", (PlayerMovement.player.position + (Vector3.up * (PlayerMovement.player.localScale.y / 8f))) + (fpsCam.transform.TransformDirection(Vector3.forward).normalized * 2.5f), Quaternion.Euler(fpsCam.transform.TransformDirection(Vector3.forward)), fpsCam.transform.TransformDirection(Vector3.forward).normalized, shooter.chargeBullet, null);
                obj.GetComponentInChildren<ExplosiveShot>().comboSize = gunCharge;
                break;
        }
        onChargedShotFired?.Raise(gunCharge);
        SetCharge(0);
    }

    public void Bounce(int bounceCount) 
    {
        SetCharge(bounceCount);
        ChargedFeedback?.StopFeedbacks(); chargedShotPS.Clear();
    }

    public void Catch()
    {
        ChargedFeedback?.PlayFeedbacks();
    }

    public void Reset()
    {
        ChargedFeedback?.StopFeedbacks();
        chargedShotPS.Clear();
        anim.SetInteger("ChargesLeft", gunCharge);
        SetCharge(0);
    }

    public void Dropped() 
    {
        if (!transform.parent)  //if the gun is dropped and has no parent
        {
            Reset();
        }
    }

    private int DamageAmountCalc(int charges)
    {
        foreach (Vector2 amount in shooter.damageGraph)  //loops through the vector 2 (graph)
        {
            if (amount.x == charges)
            {
                return Mathf.RoundToInt(amount.y);
            }
        }
        if (charges >= shooter.damageGraph.Length) //in case you over the max
        {
            return Mathf.RoundToInt(shooter.damageGraph[shooter.damageGraph.Length - 1].y);
        }
        return 0;
    }

    private void Healing() 
    {
        if (gunCharge > 0 && Health.ReturnHealthNotMax()) 
        {
            SetCharge(gunCharge - 1);   // Minus 1 from gunCharge
            //call a heal function

            OnActivated?.Invoke(shooter.healAmount);

            if (gunCharge == 0)
            {
                Reset();
            }
        }
    }
    private void StopShooting()
    {
        repeatedShooting = false;
    }
}
