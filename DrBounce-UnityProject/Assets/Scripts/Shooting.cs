using UnityEngine;
using MoreMountains.Feedbacks;
using MoreMountains.NiceVibrations;

public class Shooting : MonoBehaviour
{
    //Events
    public delegate void Activated(int value);
    public static event Activated OnActivated;

    public enum GunModes
    {
        Explosives,
    }

    [SerializeField] private Gun shooter = null;
    [SerializeField] private GameEventBool onEnemyHover = null;
    private ObjectPooler pool;
    public InputMaster controls;
    public Camera fpsCam;
    public Animator anim;

    [Header("Damage")]
    [HideInInspector]
    public int damage = 10;     //current damage value

    [Header("Charges")]
    private int gunCharge = 0;    //amount of times the gun has been bounced successfully
    private int gunEnergy = 0;    //current amount of charges left in the gun (reset if dropped)

    [Header("Fire Rate")]
    private float timeSinceLastShot = 0;

    #region Events
    [Header("Events")]

    [SerializeField]
    private GameEvent onUnchargedShot = null;

    [SerializeField]
    [Tooltip("Passes amountOfBounces")]
    private GameEventInt onChargedShotCombo = null;

    [SerializeField]
    private GameEvent onChargedShotFired = null;

    [SerializeField]
    [Tooltip("Passes amountOfBounces")]
    private GameEventInt onBounce = null;

    [SerializeField]
    [Tooltip("Passes chargesLeft")]
    private GameEventInt onChargeUpdate = null;

    [SerializeField]
    private GameEvent onCatch = null;

    [SerializeField]
    private GameEvent onChargesEmpty = null;

    #endregion

    [Header("Feedbacks")]
    public MMFeedbacks BasicShootFeedback;
    public MMFeedbacks ChargedFeedback;
    public MMFeedbacks FirstChargedShotFeedback;
    public MMFeedbacks HoverOverFeedback;
    public MMFeedbacks RegularReticleFeedback;
    public MMFeedbacks LoseChargeFeedback;
    [SerializeField] ParticleSystem chargedShotPS;

    [Header("Vibrations")]
    public VibrationManager vibrationManager;

    // Start is called before the first frame update
    void Start()
    {
        pool = ObjectPooler.Instance;
        onEnemyHover?.Raise(false);
    }

    // Update is called once per frame
    void Update()
    {
        //print(amountOfBounces);

        timeSinceLastShot += Time.deltaTime;
        CheckifCharged();

        HoverOverEnemy();
    }

    private void Awake()
    {
        controls = new InputMaster();
        controls.Player.Shoot.performed += _ => Shoot();
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

    private void HoverOverEnemy() 
    {
        RaycastHit Reticleinfo;
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out Reticleinfo, shooter.normalRange))
        {
            Enemy enemy = Reticleinfo.transform.GetComponent<Enemy>();
            if (enemy != null)
            {
                onEnemyHover?.Raise(true);
                RegularReticleFeedback?.StopFeedbacks();
                HoverOverFeedback?.PlayFeedbacks();
                //print(enemy.transform.name + " is being hovered over!");
            }
            else
            {
                onEnemyHover?.Raise(false);
                HoverOverFeedback?.StopFeedbacks();
                RegularReticleFeedback?.PlayFeedbacks();
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
            timeSinceLastShot = 0;

            if(gunEnergy > 0) HandleComboShot();

            // Is it an uncharged/basic shot?
            else if (gunEnergy <= 0)
            {
                onUnchargedShot?.Raise();

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

                BasicShootFeedback?.PlayFeedbacks();
            }
            vibrationManager.BasicShotVibration();

            if(gunEnergy <= 0)
            {
                onChargesEmpty?.Raise();
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
                vibrationManager.ChargedShotVibration();

                shooter.chargeBullet.damage = DamageAmountCalc(gunCharge);

                GameObject obj = pool.SpawnBulletFromPool("ExplosiveShot", (PlayerMovement.player.position + (Vector3.up * (PlayerMovement.player.localScale.y / 8f))) + (fpsCam.transform.TransformDirection(Vector3.forward).normalized * 2.5f), Quaternion.Euler(fpsCam.transform.TransformDirection(Vector3.forward)), fpsCam.transform.TransformDirection(Vector3.forward).normalized, shooter.chargeBullet, null);
                obj.GetComponent<ExplosiveShot>().comboSize = gunCharge;
                //Reset();
                AddCharge(-1);
                gunCharge = 0;
                break;
        }

        onChargedShotCombo?.Raise(gunCharge);
        onChargedShotFired?.Raise();
    }

    public void Bounce(int bounceCount) 
    {
        gunCharge = bounceCount;
        ChargedFeedback?.StopFeedbacks(); chargedShotPS.Clear();
    }

    public void Catch()
    {
        AddCharge(-gunEnergy + shooter.energyGivenAfterBounce); // Set chargesLeft = shooter.amountOfChargesGiven
        ChargedFeedback?.PlayFeedbacks();
    }

    private void CheckifCharged()
    {
        if (gunEnergy <= 0)
        {
            ChargedFeedback?.StopFeedbacks(); chargedShotPS.Clear();
            MMVibrationManager.StopContinuousHaptic();
        }

        if (gunEnergy >= 1)
        {
            ChargedFeedback?.PlayFeedbacks();
            vibrationManager.ActiveChargeVibration();
        }

        anim.SetInteger("ChargesLeft", gunEnergy);
    }

    public void Reset()
    {
        ChargedFeedback?.StopFeedbacks(); chargedShotPS.Clear();
        AddCharge(-gunEnergy); // Set chargesLeft = 0
        anim.SetInteger("ChargesLeft", gunEnergy);
        gunCharge = 0;
    }

    // Use this to update chargesLeft so it raises the onChargeUpdate event along with it
    public void AddCharge(int value)
    {
        gunEnergy += value;
        onChargeUpdate?.Raise(gunEnergy);
    }

    public void Dropped() 
    {
        if (!transform.parent)  //if the gun is dropped and has no parent
        {
            LoseChargeFeedback.PlayFeedbacks();
            vibrationManager.StopActiveCharge();
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
            gunCharge--;
            //call a heal function

            OnActivated?.Invoke(shooter.healAmount);

            if (gunCharge == 0)
            {
                LoseChargeFeedback.PlayFeedbacks();
                vibrationManager.StopActiveCharge();
                Reset();
            }
        }
    }
}
