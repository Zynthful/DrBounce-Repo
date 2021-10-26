using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;
using MoreMountains.NiceVibrations;

public class Shooting : MonoBehaviour
{
    [SerializeField] private Gun shooter = null;
    //[SerializeField] private GameObject bullet;

    public delegate void Activated(int value);
    public static event Activated OnActivated;

    public enum GunModes
    {
        Explosives,
    }

    //[SerializeField] private GunModes currentGunMode;

    private ObjectPooler pool;

    public InputMaster controls;

    private float range = 100f;
    public Camera fpsCam;

    [Header("Damage")]
    public int damage = 10;     //current damage value

    [Header("Charges")]
    private int amountOfBounces = 0;    //amount of times the gun has been bounced successfully
    private int chargesLeft = 0;    //current amount of charges left in the gun (reset if dropped)

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

    [SerializeField] private GameEventBool onEnemyHover = null;

    public Animator anim;

    [SerializeField] private int healAmount = 30;

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

        RaycastHit Reticleinfo;
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out Reticleinfo, range))
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

    private void Shoot() 
    {
        // Checks:
        //  - Is not paused
        //  - Object has a parent
        //  - Is not already shooting
        if (!GameManager.s_Instance.paused && transform.parent != null && timeSinceLastShot > shooter.fireRate)  
        {
            timeSinceLastShot = 0;

            if(chargesLeft > 0) HandleComboShot();

            // Is it an uncharged/basic shot?
            else if (chargesLeft <= 0)
            {
                onUnchargedShot?.Raise();

                //ChargedFeedback?.StopFeedbacks();
                damage = Mathf.RoundToInt(shooter.damageGraph[0].y);

                RaycastHit Hitinfo;
                if(Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out Hitinfo, range))
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

            if(chargesLeft <= 0)
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

                shooter.chargeBullet.damage = DamageAmountCalc(amountOfBounces);

                GameObject obj = pool.SpawnBulletFromPool("ExplosiveShot", (PlayerMovement.player.position + (Vector3.up * (PlayerMovement.player.localScale.y / 8f))) + (fpsCam.transform.TransformDirection(Vector3.forward).normalized * 2.5f), Quaternion.Euler(fpsCam.transform.TransformDirection(Vector3.forward)), fpsCam.transform.TransformDirection(Vector3.forward).normalized, shooter.chargeBullet, null);
                obj.GetComponent<ExplosiveShot>().comboSize = amountOfBounces;
                //Reset();
                AddCharge(-1);
                amountOfBounces = 0;
                break;
        }

        onChargedShotCombo?.Raise(amountOfBounces);
        onChargedShotFired?.Raise();
    }

    public void Bounce(int bounceCount) 
    {
        amountOfBounces = bounceCount;
        ChargedFeedback?.StopFeedbacks(); chargedShotPS.Clear();
    }

    public void Catch()
    {
        AddCharge(-chargesLeft + shooter.amountOfChargesGiven); // Set chargesLeft = shooter.amountOfChargesGiven
        ChargedFeedback?.PlayFeedbacks();
    }

    private void CheckifCharged()
    {
        if (chargesLeft <= 0)
        {
            ChargedFeedback?.StopFeedbacks(); chargedShotPS.Clear();
            MMVibrationManager.StopContinuousHaptic();
        }

        if (chargesLeft >= 1)
        {
            ChargedFeedback?.PlayFeedbacks();
            vibrationManager.ActiveChargeVibration();
        }

        anim.SetInteger("ChargesLeft", chargesLeft);
    }

    public void Reset()
    {
        ChargedFeedback?.StopFeedbacks(); chargedShotPS.Clear();
        AddCharge(-chargesLeft); // Set chargesLeft = 0
        anim.SetInteger("ChargesLeft", chargesLeft);
        amountOfBounces = 0;
    }

    // Use this to update chargesLeft so it raises the onChargeUpdate event along with it
    public void AddCharge(int value)
    {
        chargesLeft += value;
        onChargeUpdate?.Raise(chargesLeft);
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
        if (amountOfBounces > 0 && Health.ReturnHealthNotMax()) 
        {
            amountOfBounces--;
            //call a heal function

            OnActivated?.Invoke(healAmount);

            if (amountOfBounces == 0)
            {
                LoseChargeFeedback.PlayFeedbacks();
                vibrationManager.StopActiveCharge();
                Reset();
            }
        }
    }
}
