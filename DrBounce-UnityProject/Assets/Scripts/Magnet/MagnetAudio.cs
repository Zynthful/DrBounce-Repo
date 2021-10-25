using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagnetAudio : MonoBehaviour
{
    [SerializeField]
    private AK.Wwise.Event recallEvent = null;

    public void PlayRecall()
    {
        recallEvent.Post(gameObject);
    }
}
