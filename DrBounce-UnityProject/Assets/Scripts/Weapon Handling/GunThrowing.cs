using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;
using MoreMountains.Feedbacks;

public class GunThrowing : MonoBehaviour
{
    [Header("Declarations")]
    [SerializeField] MagnetAssist magnet = null;
    [SerializeField] BoxCollider catchCollider = null;
    [SerializeField] Transform weaponHolderTransform = null;

    [Header("GunThrowing Settings")]
    [SerializeField] bool startOnPlayer = true;    // Should the item start on the player?
    [SerializeField] bool canThrow;
    [SerializeField] bool returning;
    [SerializeField] float throwForceMod;
    [SerializeField] float noHitDetectAfterThrowTime;
    List<PhysicMaterial> physicMaterials = new List<PhysicMaterial> { };
    Collider[] gunColliders = null;
    [SerializeField] LayerMask throwCheckLayers;
    bool throwGunDelay;
    Transform owner; // The player
    Vector3 handPosition;
    Vector3 originPoint;
    Rigidbody rb;
    private Coroutine pickupDelayCoroutine;
    private bool pickupDelayCoroutineRunning;
    private bool throwBuffer = false;

    private bool held = false;
    public bool GetIsThrowing() { return throwing; }
    public bool GetIsHeld() { return held; }
    private void SetIsHeld(bool value)
    {
        if (held == value)
            return;

        held = value;
        onIsHeld.Invoke(value);
        shooting.onHasChargeAndIsHeld.Invoke(shooting.GetHasCharge() && value);
        shooting._onHasChargeAndIsHeld.Raise(shooting.GetHasCharge() && value);
    }

    public delegate void LeftGun();
    public static event LeftGun OnLeftGun;

    public delegate void PickedUpGun();
    public static event PickedUpGun OnPickedUpGun;

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


    [Header("Coyote Time Settings")]
    [Space(10)]
    [SerializeField] private float coyoteTimeDuration;

    [Header("Throwing Settings")]
    [SerializeField]
    [Tooltip("When throwing the gun, the Throw control is disabled for this duration in seconds.")]
    private float throwDisableTime = 0.2f;

    [Header("Loneliness Settings")]
    [SerializeField]
    [Tooltip("Duration in seconds the gun needs to be left alone on the ground in order to trigger loneliness event.")]
    private float timeToTriggerLonely = 5;

    private float timeOnGround = 0;
    private bool alone = false;


    private int amountOfBounces;
    private int amountOfBouncesUnique;  // bounces where it's not bounced against the same object twice in succession

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

    private bool pulledByMagnet = false;

    //public Outline outlineScript;

    #region Events
    [Header("Bouncing Events")]
    public UnityEvent<int> onBounce = null;

    [Header("Dropped Events")]
    public UnityEvent onDroppedPreCoyote = null;
    public UnityEvent onDropped = null;
    public UnityEvent onDroppedAndLostAllCharges = null;    // Invoked only if the item loses charges on drop
    public UnityEvent onLonely = null;                      // Invoked when left on the ground for a set duration

    [Header("Throwing Events")]
    public UnityEvent onThrown = null;
    public UnityEvent onCatch = null;
    public UnityEvent onRecall = null;
    public UnityEvent onPickup = null;
    public UnityEvent onReset = null;
    public UnityEvent<bool> onIsHeld = null;

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
    #endregion

    private void OnEnable()
    {
        // Listen for input
        InputManager.inputMaster.Player.Throw.performed += _ => SetThrowGunDelay();
        InputManager.inputMaster.Player.Throw.performed += _ => CancelThrow();
        InputManager.inputMaster.Player.Recall.performed += _ => RecallGun();
    }

    private void OnDisable()
    {
        // Stop listening for input
        InputManager.inputMaster.Player.Throw.performed -= _ => SetThrowGunDelay();
        InputManager.inputMaster.Player.Throw.performed -= _ => CancelThrow();
        InputManager.inputMaster.Player.Recall.performed -= _ => ResetScript();
    }


    void Start()
    {
        //outlineScript = GetComponentInChildren<Outline>();

        owner = GameManager.player.transform;
        rb = GetComponent<Rigidbody>();
        if(shooting == null)
        {
            shooting = GetComponent<Shooting>();
        }

        if (startOnPlayer)
        {
            //outlineScript.enabled = false;
            handPosition = transform.localPosition;
            canThrow = true;
            SetIsHeld(true);
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
            SetIsHeld(false);
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

        // Handle loneliness time
        if (!GetIsHeld() && !magnet.GetIsAssistActive()) 
        {
            timeOnGround = timeOnGround + Time.deltaTime;
            if (timeOnGround >= timeToTriggerLonely && !alone) 
            {
                alone = true;
                onLonely?.Invoke();
            }
            if (timeOnGround >= 20) 
            {
                OnLeftGun?.Invoke();
            }
        }
    }

    /// <summary>
    /// Disable throwing for a given duration in seconds.
    /// </summary>
    /// <param name="waitTime">The duration in seconds to disable throwing.</param>
    /// <returns></returns>
    private IEnumerator DisableThrowForDuration(float disabledTime)
    {
        InputManager.inputMaster.Player.Throw.Disable();
        yield return new WaitForSeconds(disabledTime);
        InputManager.inputMaster.Player.Throw.Enable();
    }

    // Delay throwing to avoid recalling immediately after throwing
    public void SetThrowGunDelay()
    {
        //float test = controls.Player.Throw.ReadValue<float>();
        //print(test);

        if (!GameManager.s_Instance.paused && InputManager.inputMaster.Player.Throw.ReadValue<float>() >= 0.5f)
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
        if (GetIsHeld())
        {
            transform.rotation = weaponHolderTransform.rotation;
        }
        else if (!exitedPlayer)
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

        if (throwGunDelay)
        {
            throwGunDelay = false;
            Thrown();
        }
    }

    public void Thrown()
    {
        if (!GameManager.s_Instance.paused && canThrow && hasLetGoOfTrigger)
        {
            throwing = true;
            SetIsHeld(false);

            shooting.CancelMaxShot();

            // Disable throwing for set duration
            StartCoroutine(DisableThrowForDuration(throwDisableTime));

            if (pickupDelayCoroutineRunning) 
            {
                StopCoroutine(pickupDelayCoroutine); 
            }

            // Stops the gun from moving through walls when thrown up against them
            Vector3 raycastOrigin = new Vector3(transform.position.x, transform.position.y, transform.position.z) - (transform.forward * transform.localScale.magnitude / 3);
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

            AffectPhysics(0.2f, 0.2f);
        }
    }

    void ResetScript()
    {
        if (!GameManager.s_Instance.paused)
        {
            if (!GetIsHeld())
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
            SetIsHeld(true);
            timeOnGround = 0;
            alone = false;

            rb.velocity = Vector3.zero;
            rb.constraints = RigidbodyConstraints.FreezeAll;
            transform.parent = weaponHolderTransform;
            transform.localPosition = handPosition;
            transform.rotation = weaponHolderTransform.rotation;
            currentVel = Vector3.zero;

            onReset?.Invoke();

            //here
            // here???
            OnPickedUpGun?.Invoke();
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
            //Debug.Log("Number of total objects: " + hitObjects.Count + "... HitObject checking is: " + hitObjects[i].hitObject.gameObject.name);
            if (hitObjects[i].hitObject.gameObject.name == check.gameObject.name)
            {
                //Debug.Log("This object: " + hitObjects[i].hitObject.gameObject.name + " ... == " + check.gameObject.name);
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

            //Debug.Log("Gotta add this now: " + collision.gameObject);
            
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

        //Debug.Log("FUrther Beyond: " + hit.gameObject.name);

        for (int i = 0; i < hitObjects.Count; i++)
        {
            if (hitObjects[i].hitObject.gameObject == hit.gameObject)
            {
                //Debug.Log("cob: " + hit.gameObject.name);

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
}
