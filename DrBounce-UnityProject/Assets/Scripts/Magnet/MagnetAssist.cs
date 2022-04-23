using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class MagnetAssist : MonoBehaviour
{
    [Header("Declarations")]
    [SerializeField]
    private GunThrowing gun = null;

    [Header("Assist Settings")]
    [SerializeField]
    [Range(0.0f, 50f)]
    private float assistMaxRange = 14.0f;
    public float GetMaxRange() { return assistMaxRange; }

    [SerializeField]
    private float assistForce = 20.0f;

    private bool assistActive = false;
    public bool GetIsAssistActive() { return assistActive; }
    private void SetAssistActive(bool value)
    {
        if (assistActive == value)
            return;

        assistActive = value;

        if (value)
        {
            onAssistStart?.Invoke();
            _onAssistStart?.Raise();

            // Trigger used unlock for the first time, if it's the first time we've done so
            if (UnlockTracker.instance.lastUnlock == UnlockTracker.UnlockTypes.Magnet && !UnlockTracker.instance.usedUnlock)
            {
                UnlockTracker.instance.UsedUnlockFirstTime();
            }
        }
        else
        {
            onAssistCancelled?.Invoke();
            _onAssistCancelled?.Raise();
        }
    }

    public float GetDistance() { return Vector3.Distance(transform.position, gun.transform.position); }

    private bool inRange = false;
    private bool pressedInput = false;
    private Rigidbody rb = null;

    [Header("Unity Events")]
    public UnityEvent onTryActivateAssist = null;
    public UnityEvent onActivateFail = null;
    public UnityEvent onAssistStart = null;
    public UnityEvent onAssistCancelled = null;
    public UnityEvent<bool> onIsActiveAndInRange = null;
    public UnityEvent onInRange = null;
    public UnityEvent onOutOfRange = null;

    [Header("Game Events")]
    public GameEvent _onTryActivateAssist = null;
    public GameEvent _onAssistStart = null;
    public GameEvent _onAssistCancelled = null;
    public GameEventBool _onIsActiveAndInRange = null;

    private void OnEnable()
    {
        InputManager.inputMaster.Player.Throw.performed += _ => OnPressInput();
        InputManager.inputMaster.Player.Throw.canceled += _ => OnCancelInput();
    }

    private void OnDisable()
    {
        InputManager.inputMaster.Player.Throw.performed -= _ => OnPressInput();
        InputManager.inputMaster.Player.Throw.canceled -= _ => OnCancelInput();
    }

    private void Start()
    {
        if (!gun)
            gun = PlayerMovement.player.GetComponentInChildren<GunThrowing>();
        if (!rb)
            rb = gun.GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        inRange = InRange();

        if (GetIsAssistActive())
        {
            // Deactivates if we're holding the gun
            if (gun.GetIsHeld())
            {
                SetAssistActive(false);
            }
            // Pull objects toward us
            else
            {
                MoveObjects();
            }
        }
        // Check if we're holding down the assist button and we can activate the assist
        else if (InputManager.inputMaster.Player.Throw.ReadValue<float>() >= 0.5f)
        {
            TryActivate();
        }
    }

    private void OnPressInput()
    {
        TryActivate();
        pressedInput = true;
    }

    private void OnCancelInput()
    {
        SetAssistActive(false);
        pressedInput = false;
    }

    /// <summary>
    /// Attempts to activate the assist, doing so successfully if we're:
    ///  - Not holding the gun
    ///  - Assist is not already active
    /// </summary>
    private void TryActivate()
    {
        onTryActivateAssist?.Invoke();
        _onTryActivateAssist?.Raise();

        if (GetIsAssistActive() || gun.GetIsHeld())
            return;

        if (inRange)
        {
            SetAssistActive(true);
        }
        else if (!pressedInput) // Prevents this from calling multiple times whilst holding down the input
        {
            onActivateFail?.Invoke();
        }
    }

    private void MoveObjects()
    {
        gun.AffectPhysics(0f, 0f);
        float speed = rb.velocity.magnitude;
        if (rb.velocity.magnitude <= .2f)
        {
            speed = 2;
        }
        speed += Time.deltaTime * assistForce;
        rb.velocity = ((transform.position - gun.transform.position).normalized * assistForce / GetDistance()).normalized * speed;
    }

    /// <summary>
    /// Checks to see if we're in range of using the magnet (and that we're not holding the gun), then updates variables and invokes relevant events accordingly.
    /// </summary>
    private bool InRange()
    {
        bool inRange = GetDistance() <= assistMaxRange && !gun.GetIsHeld();

        if (inRange && !gun.GetIsHeld())
        {
            onInRange?.Invoke();
        }
        else if (!inRange || gun.GetIsHeld())
        {
            onOutOfRange?.Invoke();
        }

        return inRange;
    }
}
