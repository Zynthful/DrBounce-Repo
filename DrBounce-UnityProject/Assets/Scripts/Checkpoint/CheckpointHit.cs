using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointHit : MonoBehaviour
{
    public delegate void CollisionActive();
    public static event CollisionActive OnCollision;

    private bool activated = false;

    public static CheckpointHit checkpointInstance = null;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && !activated)
        {
            activated = true;
            OnCollision?.Invoke();
        }
    }

    private void Awake()
    {
        if (checkpointInstance == null)
        {
            checkpointInstance = FindObjectOfType(typeof(CheckpointHit)) as CheckpointHit;
        }

        if (checkpointInstance == null)
        {
            checkpointInstance = this;
        }
        else if (checkpointInstance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(this.gameObject);
    }
}
