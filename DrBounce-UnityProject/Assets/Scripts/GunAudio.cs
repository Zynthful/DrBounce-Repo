using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunAudio : MonoBehaviour
{
    [Header("Events")]
    [SerializeField]
    private AK.Wwise.Event unchargedShotEvent = null;
    [SerializeField]
    private AK.Wwise.Event chargedShotEvent = null;
    [SerializeField]
    private AK.Wwise.Event bounceEvent = null;
    [SerializeField]
    private AK.Wwise.Event pickUpFromGroundEvent = null;
    [SerializeField]
    private AK.Wwise.Event catchEvent = null;
    [SerializeField]
    private AK.Wwise.Event recallEvent = null;

    [Header("RTPCs")]
    [SerializeField]
    private AK.Wwise.RTPC chargedDamageMultiplierRTPC = null;
    [SerializeField]
    private AK.Wwise.RTPC amountOfBouncesRTPC = null;
    [SerializeField]
    private AK.Wwise.RTPC chargesRTPC = null;

    public void PlayUnchargedShot()
    {
        unchargedShotEvent.Post(gameObject);
    }
    public void PlayChargedShot()
    {
        chargedShotEvent.Post(gameObject);
    }
    public void PlayBounce(int amountOfBounces)
    {
        amountOfBouncesRTPC.SetValue(gameObject, amountOfBounces);
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
    public void PlayRecall()
    {
        recallEvent.Post(gameObject);
    }
    public void UpdateChargedDamageMultiplierRTPC(int value)
    {
        chargedDamageMultiplierRTPC.SetValue(gameObject, value);
    }
    public void UpdateChargesRTPC(int value)
    {
        chargesRTPC.SetValue(gameObject, value);
    }
}
