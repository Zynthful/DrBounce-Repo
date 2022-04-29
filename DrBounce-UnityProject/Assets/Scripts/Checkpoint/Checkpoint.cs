using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class Checkpoint : MonoBehaviour
{
    // Info about the levels (so we can get the selected level)
    [SerializeField]
    private LevelsData levelsData = null;

    // The ID of our current checkpoint. When we hit a new checkpoint, this ID is set to the ID of the hit checkpoint (but only if the new ID is higher than our current one).
    private static int currentCheckpointID = -1;

    private void Awake()
    {
#if UNITY_EDITOR
        // Delete level save and reset current checkpoint when in editor (duct-tape solution to current checkpoint ID not being reset when entering playmode)
        SaveSystem.DeleteLevelData();
        ResetCurrentCheckpoint();
#endif
    }

    private void OnEnable()
    {
        CheckpointHit.onHit += HitCheckpoint;           // Listen for when we hit a checkpoint. Passes through the checkpoint we've hit.
        SceneManager.sceneLoaded += OnSceneLoaded;      // Listen for when we load a new scene (which we use to load level progress).
    }

    private void OnDisable()
    {
        CheckpointHit.onHit -= HitCheckpoint;
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Check if our active scene matches our current level
        if (SceneManager.GetActiveScene().name == levelsData.levels[levelsData.GetCurrentLevelIndex()].GetSceneName())
        {
            // Load level progress if a save exists
            LevelSaveData save = SaveSystem.LoadInLevel();
            if (save != null)
            {
                LoadLevelProgress(save);
            }
        }
    }

    /// <summary>
    /// Sets our current checkpoint to the new ID, if the new ID is higher. Then, saves our level progress.
    /// Called by checkpoints when hit.
    /// </summary>
    /// <param name="checkpoint">The checkpoint we've hit.</param>
    private void HitCheckpoint(CheckpointHit checkpoint)
    {
        if (checkpoint.id > currentCheckpointID)
        {
            currentCheckpointID = checkpoint.id;
            SaveLevelProgress();
        }
    }

    /// <summary>
    /// Simply reloads the active scene. Should probably remove this and use LevelsData 'ReloadLevel' method.
    /// </summary>
    public static void ReloadFromCheckpoint()
    {
        LoadingScreenManager.s_Instance.LoadScene(
            SceneManager.GetActiveScene().name,
            LoadingScreenManager.ContinueOptions.RequireInput,
            LoadingScreenManager.UnloadOptions.Manual,
            LoadingScreenManager.UnloadOptions.Manual,
            1.2f);
    }

    /// <summary>
    /// Saves our level progress (currently when we hit a checkpoint).
    /// </summary>
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

    /// <summary>
    /// Loads our level progress using the provided save data. Sets the player's position, rotation, health, charges and unlocks using the save data.
    /// </summary>
    /// <param name="data">The save data to load.</param>
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
    /// Checks if our current checkpoint ID is -1 (i.e. before we've hit any checkpoints) before triggering the elevator intro feedback.
    /// </summary>
    /// <param name="feedback"></param>
    public void ElevatorCheck(GameObject feedback)
    {
        if (currentCheckpointID == -1)
        {
            feedback.GetComponent<MoreMountains.Feedbacks.MMFeedbacks>().PlayFeedbacks();
        }
    }

    /// <summary>
    /// Resets our current checkpoint to the state of not having hit any checkpoints.
    /// </summary>
    public static void ResetCurrentCheckpoint()
    {
        currentCheckpointID = -1;
    }
}