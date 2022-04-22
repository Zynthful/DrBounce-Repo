using UnityEngine;
using UnityEngine.Events;

public class Door : MonoBehaviour
{
    private bool open = false;

    [Header("Door Settings")]

    [SerializeField]
    [Tooltip("All enemies below have to be dead for this door to open.")]
    private EnemyHealth[] enemies = null;

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
    public UnityEvent onOpen = null;
    public UnityEvent onClose = null;

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
        public Transform transformToUpdate;
        public Vector3 openPosition;
        public Quaternion openRotation;
        public Vector3 openScale;
    }

    private void OnEnable()
    {
        // Listen to enemy death event for each enemy within our list
        for (int i = 0; i < enemies.Length; i++)
        {
            if (enemies[i] != null)
            {
                enemies[i].OnDeath += CheckIfCanOpen;
            }
        }

        switch (initialState)
        {
            case InitialState.Open:
                open = true;
                foreach (OpenTransformInfo info in openTransformInfo)
                {
                    info.transformToUpdate.localPosition = info.openPosition;
                    info.transformToUpdate.localRotation = info.openRotation;
                    info.transformToUpdate.localScale = info.openScale;
                }
                break;
            case InitialState.Closed:
                open = false;
                break;
            default:
                break;
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
        // Stop listening to enemy death events
        for (int i = 0; i < enemies.Length; i++)
        {
            if (enemies[i] != null)
            {
                enemies[i].OnDeath -= CheckIfCanOpen;
            }
        }
    }

    public void SetOpen(bool value)
    {
        if (open == value)
            return;

        if (value)
            Open();
        else
            Close();
    }

    public void Open() 
    {
        if (open)
            return;

        open = true;
        onOpen?.Invoke();
    }

    public void Close() 
    {
        if (!open)
            return;

        open = false;
        onClose?.Invoke();
    }

    /// <summary>
    /// Checks if any enemies are alive. If not, the door opens, otherwise, it closes.
    /// </summary>
    private void CheckIfCanOpen() 
    {
        bool isAnEnemyAlive = false;

        foreach (EnemyHealth health in enemies) 
        {
            if (health != null)
            {
                if (!health.GetIsDead())
                {
                    isAnEnemyAlive = true;
                }
            }
        }

        SetOpen(!isAnEnemyAlive);
    }
}