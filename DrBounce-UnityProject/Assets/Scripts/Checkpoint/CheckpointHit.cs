using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointHit : MonoBehaviour
{
    // Event for when we hit this checkpoint
    public delegate void Hit(CheckpointHit checkpoint);
    public static event Hit onHit;

    [Tooltip("Unique ID for this checkpoint. Each checkpoint should be ordered ascending by ID (i.e., later checkpoints = use higher id). When we hit this checkpoint, this ID will be used to set our current checkpoint.")]
    public int id = -1;

    private void Awake()
    {
        /*
        if (Checkpoint.firstSetup)
        {
            Destroy(gameObject);
        }
        else 
        {
            DontDestroyOnLoad(this.gameObject);
        }
        */
    }

    public void OnHit()
    {
        onHit?.Invoke(this);
    }
}
