using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class Checkpoint : MonoBehaviour
{
    //need to be where the player spawns

    [SerializeField]
    private Transform[] checkpoints;

    private static int currentCheckpoint = 0;

    private int currentSceneIndex = -1;
    public static Checkpoint s_Instance = null;
    public static bool firstSetup;

    private bool levelReloadFromSave;

    [Header("Events")]
    public UnityEvent onCheckpointReached = null;
    public UnityEvent onReloadFromCheckpoint = null;

    private void Awake()
    {
        if (s_Instance == null)
        {
            s_Instance = FindObjectOfType(typeof(Checkpoint)) as Checkpoint;
        }
        if (s_Instance == null)
        {
            s_Instance = this;
        }
        else if (s_Instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(this.gameObject);
        GoToCurrentCheckpoint();
    }

    private void OnEnable()
    {
        CheckpointHit.OnCollision += ReachedNextCheckpoint;
        PlayerHealth.OnPlayerDeath += ReloadFromCheckpoint;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (levelReloadFromSave && scene.buildIndex == currentSceneIndex)
        {
            levelReloadFromSave = false;
            //Debug.Log("LevelLoadFromSave");
            LoadLevelProgress(SaveSystem.LoadInLevel());
        }
    }

    private void Start()
    {
        firstSetup = true;
    }

    private void ReachedNextCheckpoint() 
    {
        if (currentCheckpoint < checkpoints.Length - 1)
        {
            currentCheckpoint++;
        }

        SaveLevelProgress();

        onCheckpointReached?.Invoke();
    }

    private void SaveLevelProgress()
    {
        if(currentSceneIndex == -1) { currentSceneIndex = SceneManager.GetActiveScene().buildIndex; }

        // Save Level Progress
        Transform player = PlayerMovement.player;

        int[] checkpoint = GetCheckpointAndLevel();

        //Debug.Log("Data saved at level " + checkpoint[1]);

        int[] unlockFilter = new int[GameManager.s_Instance.currentSettings.Length];
        for(int i = 0; i < GameManager.s_Instance.currentSettings.Length; i++)
        {
            unlockFilter[i] = (int)GameManager.s_Instance.currentSettings[i];
            Debug.Log(unlockFilter[i]);
        }

        LevelSaveData data = new LevelSaveData(checkpoint[1], 
                                                checkpoint[0], 
                                                player.GetComponent<PlayerHealth>().GetHealth(), 
                                                new float[3]{player.position.x, player.position.y, player.position.z},
                                                new float[4]{player.rotation.x, player.rotation.y, player.rotation.z, player.rotation.w},
                                                player.GetComponentInChildren<Shooting>().GetCharges(),
                                                unlockFilter);

        SaveSystem.SaveInLevel(data);
    }

    public void ReloadFromCheckpoint()
    {
        onReloadFromCheckpoint?.Invoke();
        levelReloadFromSave = true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadLevelProgress(LevelSaveData data)
    {
        //Debug.Log("Run Load level progress");

        currentCheckpoint = data.checkpoint;

        UnlockTracker.UnlockTypes[] unlocks = new UnlockTracker.UnlockTypes[data.unlocks.Length];
        for (int i = 0; i < data.unlocks.Length; i++)
        {
            unlocks[i] = (UnlockTracker.UnlockTypes)data.unlocks[i];
            //Debug.Log("Stuffherer: " + unlocks[i]);
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

    private void GoToCurrentCheckpoint()
    {
        if (SceneManager.GetActiveScene().buildIndex != 1)  //needs to be the build index for the loading screen
        {
            if (currentSceneIndex == -1)
            {
                currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            }
            if (SceneManager.GetActiveScene().buildIndex == currentSceneIndex)
            {
                GoToCheckpoint(currentCheckpoint);
            }
            else
            {
                GameManager.s_Instance.currentSettings = null;
                s_Instance = null;
                firstSetup = false;
                currentSceneIndex = -1;

                foreach (Transform trans in checkpoints)
                {
                    Destroy(trans.gameObject);
                }
                Destroy(gameObject);
            }
        }
    }

    private void GoToCheckpoint(int checkpointIndex)
    {
        PlayerMovement.player.transform.position = new Vector3(checkpoints[currentCheckpoint].position.x, checkpoints[currentCheckpoint].position.y + 1, checkpoints[currentCheckpoint].position.z);
    }

    public int[] GetCheckpointAndLevel()
    {
        return new int[2]{currentCheckpoint,currentSceneIndex};
    }
}
