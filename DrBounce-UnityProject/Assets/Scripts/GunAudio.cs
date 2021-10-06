using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunAudio : MonoBehaviour
{
    [Header("Events")]
    [SerializeField]
    private AK.Wwise.Event shootEvent = null;
    [SerializeField]
    private AK.Wwise.Event bounceEvent = null;
    [SerializeField]
    private AK.Wwise.Event pickUpFromGroundEvent = null;
    [SerializeField]
    private AK.Wwise.Event catchEvent = null;

    [Header("RTPCs")]
    [SerializeField]
    private AK.Wwise.RTPC damageRTPC = null;
    [SerializeField]
    private AK.Wwise.RTPC amountOfBouncesRTPC = null;
    [SerializeField]
    private AK.Wwise.RTPC chargesRTPC = null;

    public void PlayShoot(int damage)
    {
        damageRTPC.SetValue(gameObject, damage);
        shootEvent.Post(gameObject);
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

    public void UpdateChargesRTPC(int value)
    {
        chargesRTPC.SetValue(gameObject, value);
    }
}
