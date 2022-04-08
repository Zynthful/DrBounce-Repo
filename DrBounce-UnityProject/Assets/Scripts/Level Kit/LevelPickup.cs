using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LevelPickup : MonoBehaviour
{

    [SerializeField]
    private Unlocks unlocksOnPickup;

    [SerializeField]
    private bool destroyOnPickup = true;
    [SerializeField]
    private float timeBeforeDestroy = .05f;

    public UnityEvent onPickup = null;

    private void OnTriggerEnter(Collider other) 
    {
        if(other.CompareTag("Player"))
        {
            onPickup.Invoke();
            UnlockTracker.instance.NewUnlocks(unlocksOnPickup.unlocks);
            if(destroyOnPickup)
                Destroy(gameObject, timeBeforeDestroy);
                Destroy(this);
        }
    }
}
