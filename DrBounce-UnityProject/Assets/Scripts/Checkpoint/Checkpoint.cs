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

    [SerializeField]
    public UnityEvent OnCheckpointReached = null;

    private void Start()
    {
        firstSetup = true;
    }

    private void OnEnable()
    {
        CheckpointHit.OnCollision += ReachedNextCheckpoint;
        PlayerHealth.OnPlayerDeath += ReloadCheckpoint;
    }

    private void ReachedNextCheckpoint() 
    {
        if (currentCheckpoint < checkpoints.Length - 1)
        {
            currentCheckpoint++;
        }

        SaveLevelProgress();

        OnCheckpointReached?.Invoke();
    }

    void SaveLevelProgress()
    {
        if(currentSceneIndex == -1) { currentSceneIndex = SceneManager.GetActiveScene().buildIndex; }

        // Save Level Progress
        Transform player = PlayerMovement.player;

        int[] checkpoint = GetCheckpointAndLevel();

        Debug.Log("Data saved at level " + checkpoint[1]);

        LevelSaveData data = new LevelSaveData(checkpoint[1], 
                                                checkpoint[0], 
                                                player.GetComponent<PlayerHealth>().GetHealth(), 
                                                new float[3]{player.position.x, player.position.y, player.position.z},
                                                new float[4]{player.rotation.x, player.rotation.y, player.rotation.z, player.rotation.w},
                                                player.GetComponentInChildren<Shooting>().GetCharges());

        SaveSystem.SaveInLevel(data);
    }

    public void LoadLevelProgress(int setCheckpoint)
    {
        currentCheckpoint = setCheckpoint;
    }

    private void ReloadCheckpoint()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
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
