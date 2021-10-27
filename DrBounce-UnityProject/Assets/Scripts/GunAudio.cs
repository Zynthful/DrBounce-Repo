using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunAudio : MonoBehaviour
{
    [Header("Wwise Events")]
    [SerializeField]
    private AK.Wwise.Event unchargedShotEvent = null;
    [SerializeField]
    private AK.Wwise.Event chargedExplosiveShotEvent = null;

    [Header("RTPCs")]
    [SerializeField]
    private AK.Wwise.RTPC chargedDamageMultiplierRTPC = null;
    [SerializeField]
    private AK.Wwise.RTPC chargesRTPC = null;

    public void PlayUnchargedShot()
    {
        unchargedShotEvent.Post(gameObject);
    }
    public void PlayChargedShot()
    {
        chargedExplosiveShotEvent.Post(gameObject);
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
