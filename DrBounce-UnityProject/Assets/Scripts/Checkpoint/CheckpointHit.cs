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

    [Tooltip("The transform information of where the player will spawn when teleporting to this checkpoint. If this is not assigned, it will use this object's transform.")]
    public Transform respawnPoint = null;

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

    public void TeleportHere()
    {
        if (respawnPoint != null)
        {
            Player.GetPlayer().transform.position = respawnPoint.position;
            Player.GetPlayer().transform.rotation = respawnPoint.rotation;
        }
        else
        {
            Player.GetPlayer().transform.position = transform.position;
            Player.GetPlayer().transform.rotation = transform.rotation;
        }
    }
}

public class CheckpointComparer : IComparer
{
    public int Compare(object x, object y)
    {
        return ((CheckpointHit)x).id - ((CheckpointHit)y).id;
    }
}