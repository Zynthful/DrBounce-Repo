using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

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
    [SerializeField]
    private string resultsScreenSceneName = "Results_SCN";

    [Header("Events")]
    public UnityEvent onLevelComplete = null;
    public UnityEvent onResultsLoadComplete = null;

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
        SaveSystem.DeleteLevelData();
        GameManager.s_Instance.currentSettings = null;
        onComplete.Invoke();
        onLevelComplete?.Invoke();
    }

    public void ShowResults()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(resultsScreenSceneName, LoadSceneMode.Additive);
        operation.completed += _ =>
        {
            onResultsLoadComplete.Invoke();
        };
    }
}