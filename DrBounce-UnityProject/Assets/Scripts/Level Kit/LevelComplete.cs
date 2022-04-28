using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class LevelComplete : MonoBehaviour
{
    [Header("Level Completion Settings")]
    [SerializeField]
    private string resultsScreenSceneName = "Results_SCN";

    [Header("Events")]
    public UnityEvent onLevelComplete = null;
    public UnityEvent onResultsLoadComplete = null;

    public delegate void LevelCompleted();
    public static event LevelCompleted onComplete;

    /// <summary>
    /// Completes level and invokes level completion events.
    /// </summary>
    public void Complete() 
    {
        SaveSystem.DeleteLevelData();
        GameManager.s_Instance.currentSettings = null;
        onComplete.Invoke();
        onLevelComplete?.Invoke();
    }

    public void ShowResults()
    {
        if (!SceneManagement.IsSceneLoaded(resultsScreenSceneName))
        {
            AsyncOperation operation = SceneManager.LoadSceneAsync(resultsScreenSceneName, LoadSceneMode.Additive);
            operation.completed += _ =>
            {
                GameManager.SetCursorEnabled(true);
                PauseHandler.SetCanPause(false);
                onResultsLoadComplete.Invoke();
            };
        }
    }
}