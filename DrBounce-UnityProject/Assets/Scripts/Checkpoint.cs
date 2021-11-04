using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    //need to be where the player spawns

    [SerializeField] private GameObject player;     //needs to be manually set
    [SerializeField] private Transform[] checkpoints;

    private int currentCheckpoint = 0;


    private void ReturnToCheckpoint() 
    {
        //print("return");

        player.transform.position = checkpoints[currentCheckpoint].position;
    }

    private void ReachedNextCheckpoint() 
    {
        //print("hit me");

        if (currentCheckpoint < checkpoints.Length)
        {
            currentCheckpoint++;
        }
    }

    void OnEnable()
    {
        CheckpointHit.OnCollision += ReachedNextCheckpoint;
        DeathZone.OnPlayerDeath += ReturnToCheckpoint;
        Health.OnPlayerDeath += ReturnToCheckpoint;
    }


    void OnDisable()
    {
        CheckpointHit.OnCollision -= ReachedNextCheckpoint;
        DeathZone.OnPlayerDeath -= ReturnToCheckpoint;
        Health.OnPlayerDeath -= ReturnToCheckpoint;
    }
}
