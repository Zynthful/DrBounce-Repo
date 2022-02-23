using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;
using MoreMountains.Feedbacks;

public class GunThrowing : MonoBehaviour
{
    [SerializeField] private float waitTime = 0.2f;

    [SerializeField] bool returning;
    [SerializeField] float throwForceMod;
    [SerializeField] bool canThrow;
    [SerializeField] Transform weaponHolderTransform = null;
    [SerializeField] bool startOnPlayer;    // Should the item start on the player or not?
    [SerializeField] float noHitDetectAfterThrowTime;
    List<PhysicMaterial> physicMaterials = new List<PhysicMaterial> { };
    Collider[] gunColliders = null;
    [SerializeField] BoxCollider catchCollider;
    [SerializeField] LayerMask throwCheckLayers;
    bool throwGunDelay;
    Transform owner; // The player
    Vector3 handPosition;
    Vector3 originPoint;
    Rigidbody rb;
    private Coroutine pickupDelayCoroutine;
    private bool pickupDelayCoroutineRunning;
    private bool throwBuffer = false;

    private bool held = true;

    // Coyote Time variables (gun collision time before drop charges)

    [System.Serializable]
    readonly struct coyote
    {
        public Collision hitObject { get; }
        public Coroutine coyoteCoroutine { get; }

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
    

    private int amountOfBounces;
    private int amountOfBouncesUnique;  // bounces where it's not bounced against the same object twice in succession

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
    private bool throwing = false;

    private float timeOnGround = 0;
    [SerializeField] private float timeLeftAlone = 5;
    private bool alone = false;

    //public Outline outlineScript;

    [Header("Unity Events")]
    [SerializeField]
    private UnityEvent<int> onBounce = null;
    [SerializeField]
    private UnityEvent onPickup = null;
    [SerializeField]
    public UnityEvent onThrown = null;
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

    private void Awake()
    {
        controls = InputManager.inputMaster;
    }

    private void OnEnable()
    {
        // Listen for input
        controls.Player.Throw.performed += _ => SetThrowGunDelay();
        controls.Player.Throw.performed += _ => CancelThrow();
        controls.Player.Recall.performed += _ => RecallGun();
    }

    private void OnDisable()
    {
        // Stop listening for input
        controls.Player.Throw.performed -= _ => SetThrowGunDelay();
        controls.Player.Throw.performed -= _ => CancelThrow();
        controls.Player.Recall.performed -= _ => ResetScript();
    }

    private IEnumerator WaitAndPrint(float waitTime)
    {
        controls.Player.Throw.Disable();

        yield return new WaitForSeconds(waitTime);

        controls.Player.Throw.Enable();
    }

    void Start()
    {
        //outlineScript = GetComponentInChildren<Outline>();

        owner = PlayerMovement.player;

        rb = GetComponent<Rigidbody>();

        if (startOnPlayer)
        {
            //outlineScript.enabled = false;
            handPosition = transform.localPosition;
            canThrow = true;
            held = true;
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
            held = false;
            alone = true;
        }

        transform.rotation = Quaternion.identity;

        gunColliders = GetComponentsInChildren<Collider>();
        if (gunColliders == new Collider[0])
        {
            gunColliders = new Collider[1] { GetComponent<Collider>() };
        }

        foreach (Collider col in gunColliders)
        {
            physicMaterials.Add(col.material);
            col.enabled = false;
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

        if (!held && !alone) 
        {
            //print("timer");
            timeOnGround = timeOnGround + Time.deltaTime;
            if (timeOnGround >= timeLeftAlone) 
            {
                print("alone");
                alone = true;
            }
        }
    }

    // Delay throwing to avoid recalling immediately after throwing
    public void SetThrowGunDelay()
    {
        //float test = controls.Player.Throw.ReadValue<float>();
        //print(test);

        if (!GameManager.s_Instance.paused && controls.Player.Throw.ReadValue<float>() >= 0.5f)
        {
            throwGunDelay = true;
        }
    }

    private void CancelThrow()
    {
        throwing = false;
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
            StartCoroutine(WaitAndPrint(waitTime));

            throwing = true;
            held = false;

            if (pickupDelayCoroutineRunning) 
            {
                StopCoroutine(pickupDelayCoroutine); 
            }

            // Stops the gun from moving through walls when thrown up against them
            Vector3 raycastOrigin = new Vector3(transform.position.x, transform.position.y, transform.position.z) - (transform.forward * transform.localScale.magnitude * 1.5f);
            RaycastHit hit;
            if(Physics.Raycast(raycastOrigin, transform.forward, out hit, 1, throwCheckLayers))
            {
                transform.position = (owner.position + hit.point) / 2;
            }

            ResetCoyoteTimes();

            foreach (Collider col in gunColliders)
            {
                col.enabled = true;
            }

            pickupDelayCoroutine = StartCoroutine(EnablePickupAfterTime(0.2f));

            StartCoroutine(delayChargeLossOnThrow());

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

            foreach (Collider col in gunColliders)
            {
                col.enabled = false;
            }

            ResetCoyoteTimes();
            exitedPlayer = false;
            returning = false;
            canThrow = true;
            inFlight = false;
            hasLetGoOfTrigger = false;
            held = true;
            timeOnGround = 0;
            alone = false;

            rb.velocity = Vector3.zero;
            rb.constraints = RigidbodyConstraints.FreezeAll;
            transform.parent = weaponHolderTransform;
            transform.localPosition = handPosition;
            transform.rotation = weaponHolderTransform.rotation;
            currentVel = Vector3.zero;

            onReset?.Invoke();

            // check if charged so it updates onHasChargeAndIsHeld -> update vibrations accordingly
            shooting.CheckIfCharged();

            //here
            // here???
        }
    }

    private void RecallGun()
    {
        if (startOnPlayer && transform.parent == null && !throwBuffer)
        {
            onRecall?.Invoke();
            _onRecall?.Raise();
            amountOfBounces = 0;
            amountOfBouncesUnique = 0;
            ResetScript();
        }
    }

    private bool EndSpecificCoyoteTime(Collision check)
    {
        for (int i = 0; i < hitObjects.Count; i++)
        {
            Debug.Log("Number of total objects: " + hitObjects.Count + "... HitObject checking is: " + hitObjects[i].hitObject.gameObject.name);
            if (hitObjects[i].hitObject.gameObject.name == check.gameObject.name)
            {
                Debug.Log("This object: " + hitObjects[i].hitObject.gameObject.name + " ... == " + check.gameObject.name);
                StopCoroutine(hitObjects[i].coyoteCoroutine);
                hitObjects.RemoveAt(i);
                return true;
            }
        }
        return false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        //occures when the gun hits the floor or a relatively flat surface, removing charge from the gun
        if (collision.transform.root != owner && collision.contacts[0].normal.normalized.y > .80f && GameManager.s_Instance.bounceableLayers != (GameManager.s_Instance.bounceableLayers | 1 << collision.gameObject.layer))
        {
            onDroppedPreCoyote?.Invoke();
            _onDroppedPreCoyote?.Raise();

            Debug.Log("Gotta add this now: " + collision.gameObject);

            // Check for/end current coyote time on this object and start a new one
            EndSpecificCoyoteTime(collision);
            var coy = new coyote(collision, StartCoroutine(CoyoteTimeForPickup(collision)));
            hitObjects.Add(coy);

            // Stop gun from sliding along the floor
            AffectPhysics(0.85f, 0f);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        //occures when the gun hits the floor or a relatively flat surface, removing charge from the gun
        if (collision.transform.root != owner && GameManager.s_Instance.bounceableLayers != (GameManager.s_Instance.bounceableLayers | 1 << collision.gameObject.layer))
        {
            // Stop coyote time when leaving collision with an object
            EndSpecificCoyoteTime(collision);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.transform.root == owner && !transform.parent && !throwBuffer)
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

        if (collision.gameObject.GetComponent<Stun>() != null)
        {
            collision.gameObject.GetComponent<Stun>().BigHit();
        }

        onBounce?.Invoke(amountOfBounces);
        _onBounce?.Raise(amountOfBounces);

        MMFeedbacks feedbacks = collision.transform.GetComponentInChildren<MMFeedbacks>();
        if (feedbacks)
            feedbacks.PlayFeedbacks();

        currentVel = rb.velocity;
    }

    public void BouncedUnique(Collision collision)
    {
        amountOfBouncesUnique++;
        Bounced(collision);
    }

    public void ChargesEmpty()
    {
        amountOfBounces = 0;
        amountOfBouncesUnique = 0;
    }

    public void ChargedShotFired()
    {
        if(amountOfBounces > 0)
            amountOfBounces--;

        if (amountOfBouncesUnique > 0)
            amountOfBouncesUnique--;
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

        Debug.Log("FUrther Beyond: " + hit.gameObject.name);

        for (int i = 0; i < hitObjects.Count; i++)
        {
            if (hitObjects[i].hitObject.gameObject == hit.gameObject)
            {
                Debug.Log("cob: " + hit.gameObject.name);

                returning = false;

                // If the item loses all charges on drop
                if (amountOfBounces != 0)
                {
                    onDroppedAndLostAllCharges?.Invoke();
                    _onDroppedAndLostAllCharges?.Raise();
                }

                amountOfBounces = 0;
                amountOfBouncesUnique = 0;
                inFlight = false;

                onDropped?.Invoke();
                _onDropped?.Raise();

                ResetCoyoteTimes();
            }
        }

    }

    public bool GetIsThrowing() { return throwing; }
    public bool GetIsHeld() { return held; }
}
