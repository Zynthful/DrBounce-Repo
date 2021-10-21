using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using MoreMountains.Feedbacks;
public class GunThrowing : MonoBehaviour
{
    [SerializeField] bool returning;
    [SerializeField] float throwForceMod;
    [SerializeField] bool canThrow;
    [SerializeField] Transform weaponHolderTransform = null;
    [SerializeField] bool startOnPlayer;    // Should the item start on the player or not?
    List<PhysicMaterial> physicMaterials = new List<PhysicMaterial> { };
    Collider[] gunColliders = null;
    [SerializeField] BoxCollider catchCollider;
    bool throwGunDelay;
    Transform owner; // The player
    Vector3 handPosition;
    Vector3 originPoint;
    Rigidbody rb;

    SwitchHeldItem inventory;
    int amountOfBounces;

    public InputMaster controls;
    public Vector3 currentVel; // Used to influence aim assist to be less snappy.
    private bool exitedPlayer; // Controls when the gun can be caught by waiting until it's left the player's hitbox

    [Header("Feedbacks")]
    public MMFeedbacks BounceFeedback;
    public MMFeedbacks CatchFeedback;
    public MMFeedbacks PickupFeedback;
    public MMFeedbacks MagnetCallFeedback;
    public MMFeedbacks BounceHitFeedback;

    [Header("Vibrations")]
    public VibrationManager vibrationManager;

    public bool inFlight;

    public Outline outlineScript;

    [Header("Events")]
    [SerializeField]
    private GameEventInt onBounce = null;
    [SerializeField]
    private GameEvent onPickup = null;
    [SerializeField]
    private GameEvent onCatch = null;
    [SerializeField]
    private GameEvent onDropped = null;
    [SerializeField]
    private GameEvent onRecall = null;

    // COMBO
    [SerializeField]
    private GameEventInt onComboUpdateCurrentNum = null;    // Passes currentComboNum
    [SerializeField]
    private GameEvent onComboStart = null; // Occurs on first bounce
    [SerializeField]
    private GameEvent onComboEnd = null; // Occurs on currentCombo set to 0

    // Number of consecutive bounces in one throw
    // Resets to 0 when: Catching, amountOfBounces resetting
    private int currentComboNum = 0;

    // Start is called before the first frame update
    void Start()
    {
        outlineScript = GetComponentInChildren<Outline>();

        owner = PlayerMovement.player;
        inventory = SwitchHeldItem.instance;

        if(startOnPlayer)
        {
            outlineScript.enabled = false;
            handPosition = transform.localPosition;
            canThrow = true;
            transform.parent = weaponHolderTransform;

        }
        else
        {
            outlineScript.enabled = true;
            handPosition = new Vector3(.4f, -.2f, .65f);
            exitedPlayer = true;
            canThrow = false;
        }

        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeAll;

        transform.rotation = Quaternion.identity;

        gunColliders = GetComponentsInChildren<Collider>();
        if(gunColliders == new Collider[0])
        {
            gunColliders = new Collider[1] {GetComponent<Collider>()};
        }

        Debug.Log(gunColliders);
        foreach(Collider col in gunColliders)
        {
            physicMaterials.Add(col.material);
        }
    }

    private void Awake()
    {
        controls = new InputMaster();
        controls.Player.RecallGun.performed += _ => RecallGun();
        controls.Player.ThrowGun.performed += _ => SetThrowGunDelay();
    }

    // Delay throwing to avoid recalling immediately after throwing
    public void SetThrowGunDelay()
    {
        if (!GameManager.s_Instance.paused)
        {
            throwGunDelay = true;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (transform.parent)
        {
            transform.rotation = weaponHolderTransform.rotation;
        }

        if (throwGunDelay)
        {
            throwGunDelay = false;
            Thrown();
        }

        if (!transform.parent && !exitedPlayer)
        {
            bool fail = false;
            foreach (Collider col in gunColliders)
            {
                if (catchCollider.bounds.Intersects(col.bounds))
                {
                    fail = true; exitedPlayer = false;
                    break;
                }
            }
            if (!fail)
            {
                exitedPlayer = true;
            }
        }
    }

    public void Thrown()
    {
        if (!GameManager.s_Instance.paused && canThrow)
        {
            canThrow = false;
            outlineScript.enabled = true;
            //when we get out of prototype we need to made the world model seperate from the fp model
            gameObject.layer = 3;
            foreach (Transform child in transform)
                child.gameObject.layer = 3;

            returning = false;
            rb.constraints = RigidbodyConstraints.None;
            transform.parent = null;
            originPoint = transform.position;
            Vector3 dir = transform.forward;
            rb.velocity = new Vector3(dir.x, dir.y + .1f, dir.z) * throwForceMod; currentVel = rb.velocity;


            AffectPhysics(0.2f, 0.2f);

            if(inventory.currentHeldTransform == transform)
            {
                inventory.currentHeldTransform = null; inventory.SwitchActiveItem();
            }
        }
    }

    void ResetScript()
    {
        if (!GameManager.s_Instance.paused)
        {
            SetComboNum(0);

            if (!transform.parent)
                throwGunDelay = false;
            outlineScript.enabled = false;
            gameObject.layer = 7;
            foreach (Transform child in transform)
                child.gameObject.layer = 7;

            exitedPlayer = false;
            returning = false;
            canThrow = true;
            inFlight = false;

            rb.velocity = Vector3.zero;
            rb.constraints = RigidbodyConstraints.FreezeAll;
            transform.parent = weaponHolderTransform;
            transform.localPosition = handPosition;
            transform.rotation = weaponHolderTransform.rotation;
            MagnetCallFeedback?.PlayFeedbacks();
            currentVel = Vector3.zero;

            inventory.OnPickupItem(transform);
        }
    }

    private void RecallGun()
    {
        if (startOnPlayer)
        {
            onRecall?.Raise();
            amountOfBounces = 0;
            ResetScript();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        //occures when the gun hits the floor or a relatively flat surface, removing charge from the gun
        if (collision.contacts[0].normal.normalized.y > .80f && GameManager.bounceableLayers != (GameManager.bounceableLayers | 1 << collision.gameObject.layer)
        {
            AffectPhysics(0.85f, 0f);
            SetComboNum(0); // Reset combo

            returning = true;
            amountOfBounces = 0;
            onDropped?.Raise();
            inFlight = false;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.transform.root == owner && !transform.parent)
        {
            if(!catchCollider)
            {
                catchCollider = other.GetComponentInChildren<BoxCollider>();
                weaponHolderTransform = WeaponSway.weaponHolderTransform;
            }

            if (returning && inFlight) { onCatch?.Raise(); CatchFeedback?.PlayFeedbacks(); vibrationManager.CatchVibration(); }
            else if (exitedPlayer) { onPickup?.Raise(); PickupFeedback?.PlayFeedbacks(); }
            else { return; }
            ResetScript();
        }
    }

    public void AffectPhysics(float dynamicFriction, float bounciness)
    {
        foreach (PhysicMaterial mat in physicMaterials)
        {
            mat.dynamicFriction = dynamicFriction; mat.bounciness = bounciness;
        }
    }

    public void Bounced(Collision collision)
    {
        returning = true;
        inFlight = true;

        amountOfBounces++; onBounce?.Raise(amountOfBounces);

        BounceFeedback?.PlayFeedbacks();
        collision.transform.GetComponentInChildren<MMFeedbacks>().PlayFeedbacks();

		// Increment combo
        SetComboNum(currentComboNum + 1);

        currentVel = rb.velocity;
    }

    public void ChargesEmpty()
    {
        amountOfBounces = 0;
    }

    public void ChargedShotFired()
    {
        if(amountOfBounces > 0)
            amountOfBounces--;
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Player.ThrowGun.performed -= _ => Thrown();
        controls.Player.RecallGun.performed -= _ => ResetScript();
        controls.Disable();
    }

    private void SetComboNum(int value)
    {
        currentComboNum = value;
        onComboUpdateCurrentNum?.Raise(value);

        // Start combo if it's the first bounce
        if (currentComboNum == 1)
        {
            onComboStart?.Raise();
        }
        // End combo if it is being reset to 0
        else if (currentComboNum <= 0)
        {
            onComboEnd?.Raise();
        }
    }
}
