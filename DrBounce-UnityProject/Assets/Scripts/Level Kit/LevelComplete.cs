using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class LevelComplete : MonoBehaviour
{
    [SerializeField]
    private LevelsData levelsData;

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
        onComplete.Invoke();
        onLevelComplete?.Invoke();

        GameSaveData data = SaveSystem.LoadGameData();
        if(data == null)
            data = new GameSaveData();
        
        // Unlock next level
        data.levelUnlocked = levelsData.GetCurrentLevelIndex() + 1;

        // Save level times
        data.lastLevelTimes[levelsData.GetCurrentLevelIndex()] = Timer.GetEndTime();
        if (Timer.GetEndTime() < data.levelPBTimes[levelsData.GetCurrentLevelIndex()] || data.levelPBTimes[levelsData.GetCurrentLevelIndex()] == 0)
        {
            data.levelPBTimes[levelsData.GetCurrentLevelIndex()] = Timer.GetEndTime();
        }

        SaveSystem.SaveGameData(data);
        SaveSystem.DeleteLevelData();

        GameManager.s_Instance.currentSettings = null;
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
                PauseHandler.SetTimeFreeze(true);
                onResultsLoadComplete.Invoke();
            };
        }
    }
}