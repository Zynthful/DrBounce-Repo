using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Checkpoint : MonoBehaviour
{
    //need to be where the player spawns

    public Transform player;     //needs to be manually set

    [SerializeField] private Transform[] checkpoints;

    private int currentCheckpoint = 0;

    private int currentSceneIndex = -1;

    public static Checkpoint checkpointManagerInstance = null;

    public static bool firstSetup;

    private void Start()
    {
        firstSetup = true;
    }


    private void ReturnToCheckpoint() 
    {
        //print("return");

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        //player = PlayerMovement.player;

        //player.transform.position = checkpoints[currentCheckpoint].position;
    }

    private void ReachedNextCheckpoint() 
    {
        //print("hit me");

        if (currentCheckpoint < checkpoints.Length - 1)
        {
            currentCheckpoint++;
        }

        SaveLevelProgress();
    }

    void SaveLevelProgress()
    {
        // Save Level Progress
        Transform player = PlayerMovement.player;

        int[] checkpoint = Checkpoint.checkpointManagerInstance.GetCheckpointAndLevel();
        LevelSaveData data = new LevelSaveData(checkpoint[1], 
                                                checkpoint[0], 
                                                player.GetComponent<PlayerHealth>().GetHealth(), 
                                                new float[3]{player.position.x, player.position.y, player.position.z},
                                                player.GetComponentInChildren<Shooting>().GetCharges());

        SaveSystem.SaveInLevel(data);
    }

    public void LoadLevelProgress(int setCheckpoint)
    {
        currentCheckpoint = setCheckpoint;
    }

    void OnEnable()
    {
        CheckpointHit.OnCollision += ReachedNextCheckpoint;
        DeathZone.OnPlayerDeath += ReturnToCheckpoint;
        PlayerHealth.OnPlayerDeath += ReturnToCheckpoint;


        SceneManager.sceneLoaded += OnSceneLoaded;
    }


    void OnDisable()
    {
        CheckpointHit.OnCollision -= ReachedNextCheckpoint;
        DeathZone.OnPlayerDeath -= ReturnToCheckpoint;
        PlayerHealth.OnPlayerDeath -= ReturnToCheckpoint;

        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Awake()
    {
        if (checkpointManagerInstance == null)
        {
            checkpointManagerInstance = FindObjectOfType(typeof(Checkpoint)) as Checkpoint;
        }

        if (checkpointManagerInstance == null)
        {
            checkpointManagerInstance = this;
        }
        else if (checkpointManagerInstance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(this.gameObject);

        currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode) 
    {
        if (currentSceneIndex == -1)
        {
            currentSceneIndex = scene.buildIndex;
        }

        //makes it so you only respawn in the current scene
        if (scene.buildIndex == currentSceneIndex)
        {
            player = PlayerMovement.player;
            player.transform.position = checkpoints[currentCheckpoint].position;
        }
        else 
        {
            checkpointManagerInstance = null;
            firstSetup = false;
            //currentSceneIndex = -1;

            foreach (Transform trans in checkpoints) 
            {
                Destroy(trans.gameObject);
            }

            Destroy(gameObject);
        }
    }

    public int[] GetCheckpointAndLevel()
    {
        return new int[2]{currentCheckpoint,currentSceneIndex};
    }
}
