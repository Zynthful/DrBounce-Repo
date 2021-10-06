using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;

public class Shooting : MonoBehaviour
{
    [SerializeField] private Gun shooter = null;
    [SerializeField] private GameObject bullet;

    public enum GunModes
    {
        Basic,
        Explosives,
    }

    [SerializeField] private GunModes currentGunMode;

    private ObjectPooler pool;

    [SerializeField] BulletType explosiveShotType;

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


    [Header("Events")]
    // Passes damage fired
    [SerializeField]
    private GameEventInt onShoot = null;
    // Passes amountOfBounces
    [SerializeField]
    private GameEventInt onBounce = null;
    // Passes chargesLeft
    [SerializeField]
    private GameEventInt onGainCharge = null;


    [Header("Feedbacks")]
    public MMFeedbacks BasicShootFeedback;
    public MMFeedbacks ChargedFeedback;
    public MMFeedbacks FirstChargedShotFeedback;
    public MMFeedbacks HoverOverFeedback;
    public MMFeedbacks RegularReticleFeedback;
    [SerializeField] ParticleSystem chargedShotPS;


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
                HoverOverFeedback?.PlayFeedbacks();
                print(enemy.transform.name + " is being hovered over!");
            }
            else
            {
                RegularReticleFeedback?.PlayFeedbacks();
            }
        }
    }

    private void Awake()
    {
        controls = new InputMaster();
        controls.Player.Shoot.performed += _ => Shoot();
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

            if(chargesLeft > 0)
                HandleComboShot();
                
            else if(currentGunMode == GunModes.Basic || chargesLeft <= 0)
            {
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
            }
            BasicShootFeedback?.PlayFeedbacks();
            
            //Instantiate(bullet, transform.position, transform.rotation, null); Change to use raycast

            onShoot?.Raise(damage);

            //Debug.LogWarning("BANG!!!");
            //Debug.LogWarning("You shot for " + damage);
        }
    }

    private void HandleComboShot()
    {
    	ChargedFeedback?.PlayFeedbacks();
    	
        switch(currentGunMode){
            case GunModes.Basic:
                damage = (int)(shooter.baseDamage * amountOfBounces * shooter.damageModifier);
                chargesLeft--;
                break;

            case GunModes.Explosives:
                FirstChargedShotFeedback?.PlayFeedbacks();
                GameObject obj = pool.SpawnBulletFromPool("ExplosiveShot", transform.position + transform.TransformDirection(Vector3.forward).normalized * 2.5f, Quaternion.identity, transform.TransformDirection(Vector3.forward).normalized, explosiveShotType, null);
                obj.GetComponent<ExplosiveShot>().comboSize = amountOfBounces;
                Reset();
                break;
        }
    }

    public void Bounce() 
    {
        amountOfBounces++;
        ChargedFeedback?.StopFeedbacks(); chargedShotPS.Clear();

        onBounce?.Raise(amountOfBounces);
        onGainCharge?.Raise(chargesLeft);
    }

    public void Catch()
    {
        chargesLeft = shooter.amountOfChargesGiven;
        ChargedFeedback?.PlayFeedbacks();
    }

    private void CheckifCharged()
    {
        if (chargesLeft <= 0)
        {
            anim.SetTrigger("NoCharge");
            ChargedFeedback?.StopFeedbacks(); chargedShotPS.Clear();
        }

        if (chargesLeft >= 1)
        {
            ChargedFeedback?.PlayFeedbacks();
        }
    }

    public void Reset()
    {
        ChargedFeedback?.StopFeedbacks(); chargedShotPS.Clear();
        anim.SetTrigger("NoCharge");
        amountOfBounces = 0;
        chargesLeft = 0;
    }
}
