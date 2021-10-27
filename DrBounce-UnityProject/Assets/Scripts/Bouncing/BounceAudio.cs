using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BounceAudio : MonoBehaviour
{
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
    [SerializeField]
    private AK.Wwise.RTPC chargesRTPC = null;

    public void PlayBounce(int amountOfBounces)
    {
        numOfBouncesRTPC.SetValue(gameObject, amountOfBounces);
        bounceEvent.Post(gameObject);
    }
    public void PlayPickUp()
    {
        pickUpFromGroundEvent.Post(gameObject);
    }
    public void PlayCatch()
    {
        catchEvent.Post(gameObject);
    }
        public void UpdateChargesRTPC(int value)
    {
        chargesRTPC.SetValue(gameObject, value);
    }
}
