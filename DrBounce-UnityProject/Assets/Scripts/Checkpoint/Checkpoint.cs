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
    private string sceneName = "";

    public static Checkpoint checkpointManagerInstance = null;

    public static bool firstSetup;

    private void Start()
    {
        firstSetup = true;
    }


    private void ReturnToCheckpoint() 
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void ReachedNextCheckpoint() 
    {
        if (currentCheckpoint < checkpoints.Length - 1)
        {
            currentCheckpoint++;
        }
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
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.buildIndex != 1)  //needs to be the build index for the loading screen
        {
            if (currentSceneIndex == -1)
            {
                currentSceneIndex = scene.buildIndex;
            }
            if (scene.buildIndex == currentSceneIndex)
            {
                player = PlayerMovement.player;
                player.transform.position = new Vector3(checkpoints[currentCheckpoint].position.x, checkpoints[currentCheckpoint].position.y + 1, checkpoints[currentCheckpoint].position.z);
            }
            else
            {
                checkpointManagerInstance = null;
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
}
