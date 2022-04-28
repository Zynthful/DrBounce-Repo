using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointHit : MonoBehaviour
{
    public delegate void Hit(CheckpointHit checkpoint);
    public static event Hit onHit;

    [Tooltip("Unique ID for this checkpoint. Each checkpoint should be ordered ascending by ID (i.e., later checkpoints = use higher id).")]
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
