using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;

public class Shooting : MonoBehaviour
{
    [SerializeField] private Gun shooter = null;
    [SerializeField] private GameObject bullet;

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
    [SerializeField]
    private GameEventInt onShoot = null;

    [Header("Feedbacks")]
    public MMFeedbacks BasicShootFeedback;
    public MMFeedbacks ChargedFeedback;

    public Animator anim;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        timeSinceLastShot += Time.deltaTime;
        CheckifCharged();
    }

    private void Awake()
    {
        controls = new InputMaster();
        controls.Player.Shoot.performed += _ => Shoot();
    }

    private void OnEnable()
    {
        controls.Enable();
        GunBounce.OnPickUp += Bounce;
        GunBounce.OnFloorCollision += Reset;
    }

    private void OnDisable()
    {
        controls.Disable();
        GunBounce.OnPickUp -= Bounce;
        GunBounce.OnFloorCollision += Reset;
    }

    private void Shoot() 
    {
        if (transform.parent != null && timeSinceLastShot > shooter.fireRate)    //checks the object has a parent and is not already shooting
        {
            timeSinceLastShot = 0;

            if (chargesLeft > 0)
            {
                ChargedFeedback?.PlayFeedbacks();
                damage = (int)(shooter.baseDamage * amountOfBounces * shooter.damageModifier);
                chargesLeft--;
            }
            else 
            {
                //ChargedFeedback?.StopFeedbacks();
                damage = shooter.baseDamage;

            }
            BasicShootFeedback?.PlayFeedbacks();
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
            //Instantiate(bullet, transform.position, transform.rotation, null); Change to use raycast

            onShoot?.Raise(damage);

            //Debug.LogWarning("BANG!!!");
            //Debug.LogWarning("You shot for " + damage);
        }
    }

    private void Bounce() 
    {
        amountOfBounces++;
        ChargedFeedback?.StopFeedbacks();
        ChargedFeedback?.PlayFeedbacks();
        chargesLeft = shooter.amountOfChargesGiven;
    }

    private void CheckifCharged()
    {
        if (chargesLeft <= 0)
        {
            anim.SetTrigger("NoCharge");
            ChargedFeedback?.StopFeedbacks();
        }

        if (chargesLeft >= 1)
        {
            ChargedFeedback?.PlayFeedbacks();
        }
    }

    private void Reset()
    {
        amountOfBounces = 0;
        chargesLeft = 0;
    }
}