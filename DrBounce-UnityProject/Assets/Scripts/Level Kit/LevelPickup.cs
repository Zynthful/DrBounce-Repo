using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelPickup : MonoBehaviour
{

    [SerializeField]
    private Unlocks unlocksOnPickup;

    [SerializeField]
    private bool destroyOnPickup = true;
    [SerializeField]
    private float timeBeforeDestroy = .2f;

    private void OnTriggerEnter(Collider other) 
    {
        if(other.CompareTag("Player"))
        {
            GetComponent<UnlockTracker>().NewUnlocks(unlocksOnPickup.unlocks);
            if(destroyOnPickup)
                Destroy(gameObject, timeBeforeDestroy);
                Destroy(this);
        }
    }
}
