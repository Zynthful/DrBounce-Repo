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

    public static Checkpoint checkpointManagerInstance = null;

    public static bool firstSetup;

    private void Start()
    {
        firstSetup = true;
    }


    private void ReturnToCheckpoint() 
    {
        print("return");

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        //player = PlayerMovement.player;

        //player.transform.position = checkpoints[currentCheckpoint].position;
    }

    private void ReachedNextCheckpoint() 
    {
        print("hit me");

        if (currentCheckpoint < checkpoints.Length - 1)
        {
            currentCheckpoint++;
            print(currentCheckpoint);
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
        player = PlayerMovement.player;
        player.transform.position = checkpoints[currentCheckpoint].position;
    }
}
