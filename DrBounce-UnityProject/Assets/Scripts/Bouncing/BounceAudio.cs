using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BounceAudio : MonoBehaviour
{
    [SerializeField]
    private GameObject objToPost = null;
    
    [Header("Wwise Events")]
    [SerializeField]
    private AK.Wwise.Event bounceEvent = null;
    [SerializeField]
    private AK.Wwise.Event pickUpFromGroundEvent = null;
    [SerializeField]
    private AK.Wwise.Event catchEvent = null;

    [Header("RTPCs")]
    [SerializeField]
    private AK.Wwise.RTPC numOfBouncesRTPC = null;

    public void PlayBounce(int amountOfBounces)
    {
        numOfBouncesRTPC.SetValue(objToPost, amountOfBounces);
        bounceEvent.Post(objToPost);
    }
    public void PlayPickUp()
    {
        pickUpFromGroundEvent.Post(objToPost);
    }
    public void PlayCatch()
    {
        catchEvent.Post(objToPost);
    }
}
