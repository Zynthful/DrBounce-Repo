using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "Levels Data", menuName = "ScriptableObjects/Levels/Levels Data")]
public class LevelsData : ScriptableObject
{
    public List<LevelData> levels;

    private int currentLevelIndex = -1;
    public int GetCurrentLevelIndex() { return currentLevelIndex; }

    // Only need to update our current level in editor
#if UNITY_EDITOR
    private void OnEnable()
    {
        UpdateCurrentLevel();
    }
#endif

    private void UpdateCurrentLevel()
    {
        //currentLevel = null;
        currentLevelIndex = -1;
        for (int i = 0; i < levels.Count; i++)
        {
            if (levels[i].GetSceneName() == SceneManager.GetActiveScene().name)
            {
                //currentLevel = levels[i];
                currentLevelIndex = i;
            }
        }
    }

    public void LoadLevel(int levelIndex)
    {
        LoadLevel(levels[levelIndex]);
    }

    public void LoadLevel(LevelData level)
    {
        if (IsLevelValid(level))
        {
            //currentLevel = level;
            currentLevelIndex = levels.IndexOf(level);
            LoadingScreenManager.s_Instance.LoadScene(level.GetSceneName(), LoadingScreenManager.ContinueOptions.RequireInput, LoadingScreenManager.UnloadOptions.Manual, LoadingScreenManager.UnloadOptions.Manual, 1.2f);
        }
        else
        {
            Debug.LogError($"The level {level.GetLevelName()} you're trying to load is not valid!", this);
        }
    }

    public void LoadNextLevel()
    {
        if (levels.Count > currentLevelIndex + 1)
        {
            LoadLevel(currentLevelIndex + 1);
        }
        else
        {
            // Reached last level, can't load next
        }
    }

    public bool IsLevelValid(int levelIndex)
    {
        return levels[levelIndex] != null;
    }
    
    public bool IsLevelValid(LevelData level)
    {
        return levels.Contains(level);
    }
}