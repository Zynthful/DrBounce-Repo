using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointHit : MonoBehaviour
{
    public delegate void CollisionActive();
    public static event CollisionActive OnCollision;

    private bool activated = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && !activated)
        {
            activated = true;
            OnCollision?.Invoke();
        }
    }
}
