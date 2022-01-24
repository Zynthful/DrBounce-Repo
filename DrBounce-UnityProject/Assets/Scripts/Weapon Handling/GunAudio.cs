using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunAudio : MonoBehaviour
{
    [SerializeField]
    private GameObject objToPost = null;

    [Header("Wwise Events")]
    [SerializeField]
    private AK.Wwise.Event unchargedShotEvent = null;
    [SerializeField]
    private AK.Wwise.Event chargedExplosiveShotEvent = null;
    [SerializeField]
    private AK.Wwise.Event allChargesLostEvent = null;

    [Header("RTPCs")]
    [SerializeField]
    private AK.Wwise.RTPC chargeRTPC = null;
    [SerializeField]
    private AK.Wwise.RTPC lastShotChargeRTPC = null; // Charge value which updates only on shooting - this affects the gun charged shot sound and is needed so it doesn't immediately reset to 0 on firing

    public void PlayUnchargedShot()
    {
        unchargedShotEvent.Post(objToPost);
    }
    public void PlayChargedShot()
    {
        SetLastChargeRTPC((int) chargeRTPC.GetValue(objToPost));
        chargedExplosiveShotEvent.Post(objToPost);
    }
    public void PlayChargesLost()
    {
        allChargesLostEvent.Post(objToPost);
    }

    public void SetChargesRTPC(int value)
    {
        chargeRTPC.SetValue(objToPost, value);
    }
    public void SetLastChargeRTPC(int value)
    {
        lastShotChargeRTPC.SetValue(objToPost, value);
    }
}
