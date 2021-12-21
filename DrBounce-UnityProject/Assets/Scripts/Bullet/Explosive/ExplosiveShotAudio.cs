using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveShotAudio : MonoBehaviour
{
    [SerializeField]
    private AK.Wwise.Event explodeEvent = null;

    public void PlayExplode()
    {
        explodeEvent.Post(gameObject);
    }
}