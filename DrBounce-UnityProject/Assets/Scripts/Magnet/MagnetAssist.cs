using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class MagnetAssist : MonoBehaviour
{
    private InputMaster controls = null;

    [SerializeField]
    private GunThrowing gun = null;

    public delegate void MagnetUse(bool assistActive);
    public static event MagnetUse OnMagnetUse;

    [Header("Assist Settings")]
    [SerializeField]
    [Range(0.0f, 50f)]
    private float assistMaxRange = 14.0f;
    [SerializeField]
    private float assistForce = 20.0f;

    private bool assistActive = false;
    private bool inRange = false;
    private Rigidbody rb = null;

    [Header("Unity Events")]
    [SerializeField]
    private UnityEvent onTryActivateAssist = null;
    [SerializeField]
    private UnityEvent onActivateFail = null;
    [SerializeField]
    private UnityEvent onAssistStart = null;
    [SerializeField]
    private UnityEvent onAssistCancelled = null;
    [SerializeField]
    private UnityEvent<bool> onIsActiveAndInRange = null;
    [SerializeField]
    private UnityEvent onInRange = null;
    [SerializeField]
    private UnityEvent onOutOfRange = null;

    [Header("Game Events")]
    [SerializeField]
    private GameEvent _onTryActivateAssist = null;
    [SerializeField]
    private GameEvent _onAssistStart = null;
    [SerializeField]
    private GameEvent _onAssistCancelled = null;
    [SerializeField]
    private GameEventBool _onIsActiveAndInRange = null;

    private void Awake()
    {
        controls = InputManager.inputMaster;
    }

    private void OnEnable()
    {
        controls.Player.Throw.started += _ => TryActivate();
        controls.Player.Throw.canceled += _ => TryDeactivate();
    }

    private void OnDisable()
    {
        controls.Player.Throw.started -= _ => TryActivate();
        controls.Player.Throw.canceled -= _ => TryDeactivate();
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
        CheckRange();

        // Pull objects toward us if the assist is active
        if (assistActive)
        {
            MoveObjects();
        }

        /*
        // Check if we're holding down the assist button and we can activate the assist
        if (!assistActive && !gun.transform.parent && inRange && !gun.GetIsThrowing() && controls.Player.Throw.ReadValue<float>() >= 0.5f)
        {
            assistActive = true;
            OnMagnetUse?.Invoke(assistActive);
            onAssistStart?.Invoke();
            _onAssistStart?.Raise();
        }
        // Pull objects toward us if the assist is active
        else if (assistActive)
        {
            MoveObjects();
        }
        else
        {
            TryDeactivate();
        }
        */
    }

    /// <summary>
    /// Attempts to activate the assist, doing so successfully if we're not holding the gun and the gun is in range.
    /// </summary>
    private void TryActivate()
    {
        onTryActivateAssist?.Invoke();
        _onTryActivateAssist?.Raise();

        if (!gun.transform.parent && controls.Player.Throw.ReadValue<float>() >= 0.5f)
        {
            if (inRange)
            {
                assistActive = true;
                OnMagnetUse?.Invoke(assistActive);

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
                onActivateFail?.Invoke();
            }
        }
    }

    private void TryDeactivate()
    {
        if (assistActive)
        {
            Deactivate();
        }
    }

    /// <summary>
    /// Deactivates the assist, if it was active.
    /// </summary>
    private void Deactivate()
    {
        assistActive = false;
        OnMagnetUse?.Invoke(assistActive);

        onAssistCancelled?.Invoke();
        _onAssistCancelled?.Raise();
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
    private void CheckRange()
    {
        bool wasInRange = inRange;
        inRange = GetDistance() <= assistMaxRange && !gun.GetIsHeld();

        if (inRange && !wasInRange && !gun.GetIsHeld())
        {
            onInRange?.Invoke();
        }
        else if ((!inRange || gun.GetIsHeld()) && wasInRange)
        {
            onOutOfRange?.Invoke();
        }
    }

    public float GetMaxRange()
    {
        return assistMaxRange;
    }

    public float GetDistance()
    {
        return Vector3.Distance(transform.position, gun.transform.position);
    }
}
