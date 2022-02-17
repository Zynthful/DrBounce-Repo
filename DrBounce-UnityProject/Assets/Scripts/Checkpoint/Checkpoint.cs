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
        //print("awake");

        if (checkpointManagerInstance == null)
        {
            //print("finding");
            checkpointManagerInstance = FindObjectOfType(typeof(Checkpoint)) as Checkpoint;

            //print(checkpointManagerInstance);
        }

        if (checkpointManagerInstance == null)
        {
            //print("making");
            checkpointManagerInstance = this;
        }
        else if (checkpointManagerInstance != this)
        {
            //print("deathing");
            Destroy(gameObject);
        }

        DontDestroyOnLoad(this.gameObject);

        //print("before " + currentSceneIndex);
        currentSceneIndex = SceneManager.GetActiveScene().buildIndex;   //think this line is causing the bug but don't know how to fix it
        //print("after " + currentSceneIndex);

        sceneName = SceneManager.GetActiveScene().name;
        //print(sceneName);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode) 
    {
        //print("on scene loaded");

        //print(scene.buildIndex);

        //print("work? " + currentSceneIndex);
        //print(sceneName);

        //print(scene.buildIndex == currentSceneIndex);

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
            currentSceneIndex = -1;

            foreach (Transform trans in checkpoints) 
            {
                Destroy(trans.gameObject);
            }

            //print("death");
            Destroy(gameObject);
        }
    }
}
