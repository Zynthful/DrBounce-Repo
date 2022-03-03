using UnityEngine;
using UnityEngine.Events;

public class LevelComplete : MonoBehaviour
{
    private enum Condition
    {
        Trigger,
    }

    [Header("Level Completion Settings")]
    [SerializeField]
    private Condition condition = Condition.Trigger;
    [SerializeField]
    private TriggerInvoke trigger = null;

    [Header("Events")]
    public UnityEvent onLevelComplete = null;
    public GameEvent _onLevelComplete = null;

    public delegate void LevelCompleted();
    public static event LevelCompleted onComplete;

    private void Start()
    {
        switch (condition)
        {
            case Condition.Trigger:
                // Listen for trigger detect
                trigger.onDetect.AddListener((obj) => TryComplete());
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// Attempts to run Complete, only doing so if conditions are fulfilled
    /// </summary>
    public void TryComplete()
    {
        Complete();
    }

    /// <summary>
    /// Completes level and invokes level completion events.
    /// </summary>
    private void Complete() 
    {
        onComplete?.Invoke();
        onLevelComplete?.Invoke();
        _onLevelComplete?.Raise();
    }
}
