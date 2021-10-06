using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAudio : MonoBehaviour
{
    [Header("Events")]
    [SerializeField]
    private AK.Wwise.Event gunBounceEvent = null;

    public void PlayBounce()
    {
        gunBounceEvent.Post(gameObject);
    }
}
