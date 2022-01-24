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
    [SerializeField]
    private UnityEvent onLevelComplete = null;
    [SerializeField]
    private GameEvent _onLevelComplete = null;

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
        onLevelComplete.Invoke();
        _onLevelComplete?.Raise();
    }
}
