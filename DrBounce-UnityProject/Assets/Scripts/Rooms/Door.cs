using UnityEngine;
using UnityEngine.Events;

public class Door : MonoBehaviour
{
    private bool open = false;

    [Header("Door Settings")]

    [SerializeField]
    [Tooltip("All enemies below have to be dead for this door to open.")]
    private EnemyHealth[] enemies = null;

    private int numAlive = 0;

    [SerializeField]
    [Tooltip("Whether the door should start [OPEN] or [CLOSED].")]
    private InitialState initialState = InitialState.Closed;

    [SerializeField]
    [Tooltip("If automatic, it will close when it detects an enemy is alive. If manual, it will only close when [Close()] is called.")]
    private CloseSettings closeSettings = CloseSettings.Automatic;

    [SerializeField]
    [Tooltip("The transform information (position, rotation, scale) that should be applied to the given transforms when [Initial State] is set to [OPEN].")]
    private OpenTransformInfo[] openTransformInfo;

    [Header("Unity Events")]
    public UnityEvent onInitOpen = null;
    public UnityEvent onInitNoEnemiesAlive = null;
    public UnityEvent onInitCloseWithEnemiesAlive = null;
    public UnityEvent onOpen = null;
    public UnityEvent onClose = null;
    public UnityEvent onCloseWithEnemiesAlive = null;
    public UnityEvent<int> onNumEnemiesValueChanged = null;

    [Header("Wwise")]
    [SerializeField]
    private AK.Wwise.Switch doorSwitch;

    private enum InitialState
    {
        Open,
        Closed,
    }

    private enum CloseSettings
    {
        Automatic,
        Manual,
    }

    [System.Serializable]
    private struct OpenTransformInfo
    {
        [Tooltip("The transform which should be updated on Awake if the door's initial state is set to [OPEN].")]
        public Transform transformToUpdate;
        [Tooltip("[OPTIONAL] Overrides the position, rotation, and scale.")]
        public Transform openTransform;
        public Vector3 openPosition;
        public Quaternion openRotation;
        public Vector3 openScale;
    }

    private void OnEnable()
    {
        doorSwitch.SetValue(gameObject);

        numAlive = GetNumAlive();

        if (numAlive <= 0)
            onInitNoEnemiesAlive.Invoke();

        switch (initialState)
        {
            case InitialState.Open:
                onInitOpen.Invoke();
                open = true;
                foreach (OpenTransformInfo info in openTransformInfo)
                {
                    if (info.openTransform != null)
                    {
                        info.transformToUpdate.localPosition = info.openTransform.localPosition;
                        info.transformToUpdate.localRotation = info.openTransform.localRotation;
                        info.transformToUpdate.localScale = info.openTransform.localScale;
                    }
                    else
                    {
                        info.transformToUpdate.localPosition = info.openPosition;
                        info.transformToUpdate.localRotation = info.openRotation;
                        info.transformToUpdate.localScale = info.openScale;
                    }
                }
                break;

            case InitialState.Closed:
                open = false;
                if (numAlive >= 1)
                    onInitCloseWithEnemiesAlive.Invoke();
                break;

            default:
                break;
        }


        // Listen to enemy death event for each enemy within our list
        for (int i = 0; i < enemies.Length; i++)
        {
            if (enemies[i] != null)
            {
                enemies[i].OnDeath += CheckIfCanOpen;
            }
        }

        switch (closeSettings)
        {
            case CloseSettings.Automatic:
                CheckIfCanOpen();
                break;
            case CloseSettings.Manual:
                break;
            default:
                break;
        }
    }

    private void OnDisable()
    {
        if (enemies != null)
        {
            // Stop listening to enemy death events
            for (int i = 0; i < enemies.Length; i++)
            {
                if (enemies[i] != null)
                {
                    enemies[i].OnDeath -= CheckIfCanOpen;
                }
            }
        }
    }

    public void SetOpen(bool value)
    {
        if (open == value)
            return;

        open = value;
        numAlive = GetNumAlive();

        if (value)
        {
            onOpen.Invoke();
        }
        else
        {
            onClose.Invoke();
            if (numAlive >= 1)
                onCloseWithEnemiesAlive.Invoke();
        }

    }

    public void Open()
    {
        SetOpen(true);
    }

    public void Close()
    {
        SetOpen(false);
    }

    /// <summary>
    /// Checks if any enemies are alive. If not, the door opens, otherwise, it closes.
    /// </summary>
    private void CheckIfCanOpen() 
    {
        numAlive = GetNumAlive();
        SetOpen(numAlive <= 0);
    }

    private int GetNumAlive()
    {
        int numAlive = 0;
        foreach (EnemyHealth health in enemies)
        {
            if (health != null)
            {
                if (!health.GetIsDead())
                {
                    numAlive++;
                }
            }
        }
        onNumEnemiesValueChanged.Invoke(numAlive);
        return numAlive;
    }
}