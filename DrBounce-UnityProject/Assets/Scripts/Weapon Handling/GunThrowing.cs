using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;
using MoreMountains.Feedbacks;

public class GunThrowing : MonoBehaviour
{
    [SerializeField] bool returning;
    [SerializeField] float throwForceMod;
    [SerializeField] bool canThrow;
    [SerializeField] Transform weaponHolderTransform = null;
    [SerializeField] bool startOnPlayer;    // Should the item start on the player or not?
    [SerializeField] float noHitDetectAfterThrowTime;
    List<PhysicMaterial> physicMaterials = new List<PhysicMaterial> { };
    Collider[] gunColliders = null;
    [SerializeField] BoxCollider catchCollider;
    bool throwGunDelay;
    Transform owner; // The player
    Vector3 handPosition;
    Vector3 originPoint;
    Rigidbody rb;
    private Coroutine pickupDelayCoroutine;
    private bool pickupDelayCoroutineRunning;
    private bool throwBuffer = false;


    // Coyote Time variables (gun collision time before drop charges)
    private struct coyote
    {
        public Collision hitObject;
        public Coroutine coyoteCoroutine;

        public coyote(Collision col, Coroutine cor)
        {
            hitObject = col;
            coyoteCoroutine = cor;
        }
    }
    private List<coyote> hitObjects = new List<coyote> { };
    [Header("Coyote Time")]
    [Space(10)]
    [SerializeField] private float coyoteTimeDuration;
    

    SwitchHeldItem inventory;
    int amountOfBounces;

    public InputMaster controls;
    [Space(10)]
    public Vector3 currentVel; // Used to influence aim assist to be less snappy.
    private bool exitedPlayer; // Controls when the gun can be caught by waiting until it's left the player's hitbox
    //checks if the player has let go of the tigger before throwing again, 
    //to stop spam throwing and catching on controller.
    private bool hasLetGoOfTrigger;

    [SerializeField]
    private Shooting shooting = null;

    public bool inFlight;

    //public Outline outlineScript;

    [Header("Unity Events")]
    [SerializeField]
    private UnityEvent<int> onBounce = null;
    [SerializeField]
    private UnityEvent onPickup = null;
    [SerializeField]
    private UnityEvent onThrown = null;
    [SerializeField]
    private UnityEvent onCatch = null;
    [SerializeField]
    private UnityEvent onDropped = null;
    [SerializeField]
    private UnityEvent onDroppedAndLostAllCharges = null; // Invoked only if the item loses charges on drop
    [SerializeField]
    private UnityEvent onRecall = null;
    [SerializeField]
    private UnityEvent onReset = null;
    [SerializeField]
    private UnityEvent onDroppedPreCoyote = null;

    [Header("Game Events")]
    [SerializeField]
    private GameEventInt _onBounce = null;
    [SerializeField]
    private GameEvent _onPickup = null;
    [SerializeField]
    private GameEvent _onThrown = null;
    [SerializeField]
    private GameEvent _onCatch = null;
    [SerializeField]
    private GameEvent _onDropped = null;
    [SerializeField]
    private GameEvent _onDroppedAndLostAllCharges = null; // Raised only if the item loses charges on drop
    [SerializeField]
    private GameEvent _onRecall = null;
    [SerializeField]
    private GameEvent _onDroppedPreCoyote = null;

    // Start is called before the first frame update
    void Start()
    {
        //outlineScript = GetComponentInChildren<Outline>();

        owner = PlayerMovement.player;
        inventory = SwitchHeldItem.instance;

        rb = GetComponent<Rigidbody>();

        if (startOnPlayer)
        {
            //outlineScript.enabled = false;
            handPosition = transform.localPosition;
            canThrow = true;
            transform.parent = weaponHolderTransform;
            rb.constraints = RigidbodyConstraints.FreezeAll;
        }
        else
        {
            rb.constraints = RigidbodyConstraints.None;
            //outlineScript.enabled = true;
            handPosition = new Vector3(.4f, -.2f, .65f);
            exitedPlayer = true;
            canThrow = false;
        }

        transform.rotation = Quaternion.identity;

        gunColliders = GetComponentsInChildren<Collider>();
        if(gunColliders == new Collider[0])
        {
            gunColliders = new Collider[1] {GetComponent<Collider>()};
        }

        foreach(Collider col in gunColliders)
        {
            physicMaterials.Add(col.material);
        }
    }

    private void Awake()
    {
        controls = InputManager.inputMaster;
    }

    private void OnEnable()
    {
        controls.Player.Throw.performed += _ => SetThrowGunDelay();
        controls.Player.Recall.performed += _ => RecallGun();
    }

    private void OnDisable()
    {
        controls.Player.Throw.performed -= _ => Thrown();
        controls.Player.Recall.performed -= _ => ResetScript();
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
                    fail = true;
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
        if (!GameManager.s_Instance.paused && canThrow && hasLetGoOfTrigger)
        {
            if (pickupDelayCoroutineRunning) 
            {
                StopCoroutine(pickupDelayCoroutine); 
            }
            pickupDelayCoroutine = StartCoroutine(EnablePickupAfterTime(0.2f));

            StartCoroutine(delayChargeLossOnThrow());

            ResetCoyoteTimes();
            canThrow = false;
            //outlineScript.enabled = true;
            //when we get out of prototype we need to made the world model seperate from the fp model
            gameObject.layer = 3;
            foreach (Transform child in transform)
                child.gameObject.layer = 3;

            onThrown?.Invoke();
            _onThrown?.Raise();

            returning = false;
            rb.constraints = RigidbodyConstraints.None;
            transform.parent = null;
            originPoint = transform.position;
            Vector3 dir = transform.forward;
            rb.velocity = new Vector3(dir.x, dir.y + .1f, dir.z) * throwForceMod; currentVel = rb.velocity;

            // check if charged so it updates onHasChargeAndIsHeld -> update vibrations accordingly
            shooting.CheckIfCharged();

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
            if (!transform.parent)
                throwGunDelay = false;
            //outlineScript.enabled = false;
            gameObject.layer = 7;
            foreach (Transform child in transform)
                child.gameObject.layer = 7;

            ResetCoyoteTimes();
            exitedPlayer = false;
            returning = false;
            canThrow = true;
            inFlight = false;
            hasLetGoOfTrigger = false;

            rb.velocity = Vector3.zero;
            rb.constraints = RigidbodyConstraints.FreezeAll;
            transform.parent = weaponHolderTransform;
            transform.localPosition = handPosition;
            transform.rotation = weaponHolderTransform.rotation;
            currentVel = Vector3.zero;

            onReset?.Invoke();

            inventory.OnPickupItem(transform);

            // check if charged so it updates onHasChargeAndIsHeld -> update vibrations accordingly
            shooting.CheckIfCharged();

            //here
            // here???
        }
    }

    private void RecallGun()
    {
        if (startOnPlayer && transform.parent == null)
        {
            onRecall?.Invoke();
            _onRecall?.Raise();
            amountOfBounces = 0;
            ResetScript();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        //occures when the gun hits the floor or a relatively flat surface, removing charge from the gun
        if (throwBuffer == false && collision.contacts[0].normal.normalized.y > .80f && GameManager.s_Instance.bounceableLayers != (GameManager.s_Instance.bounceableLayers | 1 << collision.gameObject.layer))
        {
            onDroppedPreCoyote?.Invoke();
            _onDroppedPreCoyote?.Raise();
            hitObjects.Add(new coyote(collision, StartCoroutine(CoyoteTimeForPickup(collision))));

            AffectPhysics(0.85f, 0f);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        //occures when the gun hits the floor or a relatively flat surface, removing charge from the gun
        if (collision.contacts[0].normal.normalized.y > .80f && GameManager.s_Instance.bounceableLayers != (GameManager.s_Instance.bounceableLayers | 1 << collision.gameObject.layer))
        {
            for(int i = 0; i < hitObjects.Count; i++)
            {
                if(hitObjects[i].hitObject == collision)
                {
                    StopCoroutine(hitObjects[i].coyoteCoroutine);
                    hitObjects.RemoveAt(i);
                    break;
                }
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.transform.root == owner && !transform.parent)
        {
            if (!catchCollider)
            {
                catchCollider = other.GetComponentInChildren<BoxCollider>();
                weaponHolderTransform = WeaponSway.weaponHolderTransform;
            }

            if (returning && inFlight)
            {
                onCatch?.Invoke();
                _onCatch?.Raise();
            }

            else if (exitedPlayer)
            {
                onPickup?.Invoke();
                _onPickup?.Raise();
            }

            else return;

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

        amountOfBounces++;
        
        onBounce?.Invoke(amountOfBounces);
        _onBounce?.Raise(amountOfBounces);

        MMFeedbacks feedbacks = collision.transform.GetComponentInChildren<MMFeedbacks>();
        if (feedbacks)
            feedbacks.PlayFeedbacks();

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

    IEnumerator EnablePickupAfterTime(float time)
    {
        pickupDelayCoroutineRunning = true;
        yield return new WaitForSeconds(time);
        exitedPlayer = true;
        pickupDelayCoroutineRunning = false;
    }

    private void ResetCoyoteTimes()
    {
        foreach (coyote coy in hitObjects)
        {
            StopCoroutine(coy.coyoteCoroutine);
        }
        hitObjects.Clear();
    }

    IEnumerator delayChargeLossOnThrow()
    {
        throwBuffer = true;
        yield return new WaitForSeconds(noHitDetectAfterThrowTime);
        throwBuffer = false;
    }

    IEnumerator CoyoteTimeForPickup(Collision hit)
    {

        yield return new WaitForSeconds(coyoteTimeDuration);
        for (int i = 0; i < hitObjects.Count; i++)
        {
            if (hitObjects[i].hitObject == hit)
            {
                returning = false;

                // If the item loses all charges on drop
                if (amountOfBounces != 0)
                {
                    onDroppedAndLostAllCharges?.Invoke();
                    _onDroppedAndLostAllCharges?.Raise();
                }

                amountOfBounces = 0;
                inFlight = false;

                onDropped?.Invoke();
                _onDropped?.Raise();

                ResetCoyoteTimes();
            }
        }

    }
    private void Update()
    {
        if (Gamepad.current != null)
        {
            if (!Gamepad.current.leftTrigger.IsActuated() && canThrow)  //checks if the player has let go of the left trigger and has the gun in hand
            {
                hasLetGoOfTrigger = true;
            }
        }
        else
        {
            hasLetGoOfTrigger = true;
        }

        //print(canThrow);
    }
}
