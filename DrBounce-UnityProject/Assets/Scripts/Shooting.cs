using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;
using MoreMountains.NiceVibrations;

public class Shooting : MonoBehaviour
{
    [SerializeField] private Gun shooter = null;
    //[SerializeField] private GameObject bullet;

    public enum GunModes
    {
        Basic,
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
    [SerializeField] ParticleSystem chargedShotPS;

    [Header("Vibrations")]
    public VibrationManager vibrationManager;


    public Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        pool = ObjectPooler.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        timeSinceLastShot += Time.deltaTime;
        CheckifCharged();

        RaycastHit Reticleinfo;
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out Reticleinfo, range))
        {
            Enemy enemy = Reticleinfo.transform.GetComponent<Enemy>();
            if (enemy != null)
            {
                RegularReticleFeedback?.StopFeedbacks();
                HoverOverFeedback?.PlayFeedbacks();
                print(enemy.transform.name + " is being hovered over!");
            }
            else
            {
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
            else if (shooter.chargeShot == GunModes.Basic || chargesLeft <= 0)
            {
                onUnchargedShot?.Raise();

                //ChargedFeedback?.StopFeedbacks();
                damage = shooter.baseDamage;
                
                RaycastHit Hitinfo;
                if(Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out Hitinfo, range))
                {
                    print(Hitinfo.transform.name + " hit!");
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
            case GunModes.Basic:
                damage = (int)(shooter.baseDamage * amountOfBounces * shooter.damageModifier);
                AddCharge(-1);
                break;

            case GunModes.Explosives:
                FirstChargedShotFeedback?.PlayFeedbacks();
                vibrationManager.ChargedShotVibration();
                GameObject obj = pool.SpawnBulletFromPool("ExplosiveShot", (PlayerMovement.player.position + (Vector3.up * (PlayerMovement.player.localScale.y / 8f))) + (fpsCam.transform.TransformDirection(Vector3.forward).normalized * 2.5f), Quaternion.Euler(fpsCam.transform.TransformDirection(Vector3.forward)), fpsCam.transform.TransformDirection(Vector3.forward).normalized, shooter.chargeBullet, null);
                obj.GetComponent<ExplosiveShot>().comboSize = amountOfBounces;
                //Reset();
                AddCharge(-1);
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
            vibrationManager.StopActiveCharge();
            Reset();
        }
    }
}
