using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class Checkpoint : MonoBehaviour
{
    // Info about the levels (so we can get the selected level)
    [SerializeField]
    private LevelsData levelsData = null;

    [SerializeField]
    private BoolSetting debugControlsEnabled = null;

    [SerializeField]
    private float invincibleTime;

    // The index of our current checkpoint from our CheckpointHit array
    private static int currentCheckpoint = -1;

    private static CheckpointHit[] checkpoints = null;
    public static CheckpointHit[] GetCheckpoints() { return checkpoints; }

#if UNITY_EDITOR
    // Whether we've ran awake or not
    private static bool doneAwake = false;
#endif

    private void Awake()
    {
        checkpoints = FindCheckpoints();

#if UNITY_EDITOR
        // When entering playmode (and running Awake for the first time) in editor, delete our save data
        if (!doneAwake)
        {
            doneAwake = true;
            SaveSystem.DeleteLevelData();
        }
#endif
    }
    private void OnEnable()
    {
        CheckpointHit.onHit += HitCheckpoint;           // Listen for when we hit a checkpoint. Passes through the checkpoint we've hit.
        SceneManager.sceneLoaded += OnSceneLoaded;      // Listen for when we load a new scene (which we use to load level progress).

        InputManager.inputMaster.Debug.DEBUG_NextCheckpoint.performed += OnNextCheckpointPerformed;
        InputManager.inputMaster.Debug.DEBUG_PrevCheckpoint.performed += OnPrevCheckpointPerformed;
    }

    private void OnDisable()
    {
        CheckpointHit.onHit -= HitCheckpoint;
        SceneManager.sceneLoaded -= OnSceneLoaded;

        InputManager.inputMaster.Debug.DEBUG_NextCheckpoint.performed -= OnNextCheckpointPerformed;
        InputManager.inputMaster.Debug.DEBUG_PrevCheckpoint.performed -= OnPrevCheckpointPerformed;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (!levelsData.IsLevelValid(levelsData.GetCurrentLevelIndex()))
            return;

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
        if (currentCheckpoint < 0 || currentCheckpoint > checkpoints.Length - 1 || checkpoint.id > checkpoints[currentCheckpoint].id)
        {
            currentCheckpoint = Array.IndexOf(checkpoints, checkpoint);
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
                                                currentCheckpoint,
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
        currentCheckpoint = data.checkpoint;

        UnlockTracker.UnlockTypes[] unlocks = new UnlockTracker.UnlockTypes[data.unlocks.Length];
        for (int i = 0; i < data.unlocks.Length; i++)
        {
            unlocks[i] = (UnlockTracker.UnlockTypes)data.unlocks[i];
        }

        Transform player = PlayerMovement.player;
        UnlockTracker tracker = player.GetComponent<UnlockTracker>();
        tracker.NewUnlocks(unlocks);
        GameManager.s_Instance.currentSettings = unlocks;

        /*
        // Set position and rotation from save data
        Vector3 newPosition = new Vector3(data.position[0], data.position[1], data.position[2]);
        Quaternion rotation = new Quaternion(data.rotation[0], data.rotation[1], data.rotation[2], data.rotation[3]);
        player.position = newPosition;
        player.rotation = rotation;
        */

        // Teleport player to checkpoint
        GoToCheckpoint(data.checkpoint);

        PlayerHealth health = player.GetComponent<PlayerHealth>();

        StartCoroutine(respawnInvincibility(health));

        data.health = health.GetMaxHealth();

        player.GetComponentInChildren<Shooting>().SetCharge(data.charges);
    }

    /// <summary>
    /// Checks if our current checkpoint ID is -1 (i.e. before we've hit any checkpoints) before triggering the elevator intro feedback.
    /// </summary>
    /// <param name="feedback"></param>
    public void ElevatorCheck(GameObject feedback)
    {
        if (currentCheckpoint == -1)
        {
            feedback.GetComponent<MoreMountains.Feedbacks.MMFeedbacks>().PlayFeedbacks();
        }
    }

    /// <summary>
    /// Resets our current checkpoint to the state of not having hit any checkpoints.
    /// </summary>
    public static void ResetCurrentCheckpoint()
    {
        currentCheckpoint = -1;
    }

    private CheckpointHit[] FindCheckpoints()
    {
        CheckpointHit[] checkpoints = FindObjectsOfType<CheckpointHit>();
        Array.Sort(checkpoints, new CheckpointComparer());
        return checkpoints;
    }

    public static void GoToCheckpoint(int index)
    {
        currentCheckpoint = index;
        checkpoints[index].TeleportHere();
    }

    IEnumerator respawnInvincibility(PlayerHealth health)
    {
        health.respawnInvincibility = true;
        yield return new WaitForSeconds(invincibleTime);
        health.respawnInvincibility = false;
    }

    private void OnNextCheckpointPerformed(InputAction.CallbackContext ctx)
    {
        if (ctx.performed && debugControlsEnabled.GetCurrentValue())
        {
            if (currentCheckpoint < checkpoints.Length - 1)
            {
                GoToCheckpoint(currentCheckpoint + 1);
            }
        }
    }

    private void OnPrevCheckpointPerformed(InputAction.CallbackContext ctx)
    {
        if (ctx.performed && debugControlsEnabled.GetCurrentValue())
        {
            if (currentCheckpoint > 0)
            {
                GoToCheckpoint(currentCheckpoint - 1);
            }
        }
    }
}