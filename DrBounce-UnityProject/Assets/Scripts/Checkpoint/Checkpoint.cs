using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class Checkpoint : MonoBehaviour
{
    [SerializeField]
    private LevelsData levelsData = null;

    private static int currentCheckpointID = -1;

    private void Awake()
    {
        /*
#if UNITY_EDITOR
        SaveSystem.DeleteLevelData();
        ResetCurrentCheckpoint();
#endif
        */
    }

    private void OnEnable()
    {
        CheckpointHit.onHit += HitCheckpoint;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        CheckpointHit.onHit -= HitCheckpoint;
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (SceneManager.GetActiveScene().name == levelsData.levels[levelsData.GetCurrentLevelIndex()].GetSceneName())
        {
            LevelSaveData save = SaveSystem.LoadInLevel();
            if (save != null)
            {
                LoadLevelProgress(save);
            }
        }
    }

    private void HitCheckpoint(CheckpointHit checkpoint)
    {
        if (checkpoint.id > currentCheckpointID)
        {
            currentCheckpointID = checkpoint.id;
            SaveLevelProgress();
        }
    }

    public static void ReloadFromCheckpoint()
    {
        LoadingScreenManager.s_Instance.LoadScene(
            SceneManager.GetActiveScene().name,
            LoadingScreenManager.ContinueOptions.RequireInput,
            LoadingScreenManager.UnloadOptions.Manual,
            LoadingScreenManager.UnloadOptions.Manual,
            1.2f);
    }

    private void SaveLevelProgress()
    {
        Transform player = Player.GetPlayer().transform;
        int[] unlockFilter = new int[GameManager.s_Instance.currentSettings.Length];
        for (int i = 0; i < GameManager.s_Instance.currentSettings.Length; i++)
        {
            unlockFilter[i] = (int)GameManager.s_Instance.currentSettings[i];
        }
        LevelSaveData data = new LevelSaveData(levelsData.GetCurrentLevelIndex(),
                                                currentCheckpointID,
                                                player.GetComponent<PlayerHealth>().GetHealth(),
                                                new float[3] { player.position.x, player.position.y, player.position.z },
                                                new float[4] { player.rotation.x, player.rotation.y, player.rotation.z, player.rotation.w },
                                                player.GetComponentInChildren<Shooting>().GetCharges(),
                                                unlockFilter);
        {

        };
        SaveSystem.SaveInLevel(data);
    }

    public void LoadLevelProgress(LevelSaveData data)
    {
        currentCheckpointID = data.checkpointID;

        UnlockTracker.UnlockTypes[] unlocks = new UnlockTracker.UnlockTypes[data.unlocks.Length];
        for (int i = 0; i < data.unlocks.Length; i++)
        {
            unlocks[i] = (UnlockTracker.UnlockTypes)data.unlocks[i];
        }

        Transform player = PlayerMovement.player;
        UnlockTracker tracker = player.GetComponent<UnlockTracker>();

        tracker.saveValues = true; tracker.saveUnlocks = unlocks;

        Vector3 newPosition = new Vector3(data.position[0], data.position[1], data.position[2]);
        Quaternion rotation = new Quaternion(data.rotation[0], data.rotation[1], data.rotation[2], data.rotation[3]);
        player.position = newPosition;
        player.rotation = rotation;

        PlayerHealth health = player.GetComponent<PlayerHealth>();
        health.saveDamageValue = player.GetComponent<PlayerHealth>().GetMaxHealth() - data.health;
        health.saveDamage = true;

        player.GetComponentInChildren<Shooting>().SetCharge(data.charges);
    }

    /// <summary>
    /// Checks if our current checkpoint is 0 before triggering the elevator intro feedback
    /// </summary>
    /// <param name="feedback"></param>
    public void ElevatorCheck(GameObject feedback)
    {
        if (currentCheckpointID == -1)
        {
            feedback.GetComponent<MoreMountains.Feedbacks.MMFeedbacks>().PlayFeedbacks();
        }
    }

    public static void ResetCurrentCheckpoint()
    {
        currentCheckpointID = -1;
    }
}