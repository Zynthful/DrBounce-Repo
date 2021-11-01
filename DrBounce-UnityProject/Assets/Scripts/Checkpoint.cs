using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [SerializeField] private Transform[] checkpoints;
    private int currentCheckpoint = -1;

    [SerializeField] private GameObject player;
    private InputMaster controls;

    private void Awake()
    {
        controls = new InputMaster();
        controls.Player.Shoot.performed += _ => ReturnToCheckpoint();
    }

    private void ReturnToCheckpoint() 
    {
        print("return");

        player.transform.position = checkpoints[currentCheckpoint].position;

        //set player position to there last currentpoint
    }

    private void ReachedNextCheckpoint() 
    {
        print("hit me");

        if (currentCheckpoint < checkpoints.Length)
        {
            currentCheckpoint++;
        }
    }

    void OnEnable()
    {
        controls.Enable();
        CheckpointHit.OnCollision += ReachedNextCheckpoint;
    }


    void OnDisable()
    {
        controls.Disable();
        CheckpointHit.OnCollision += ReachedNextCheckpoint;
    }
}
