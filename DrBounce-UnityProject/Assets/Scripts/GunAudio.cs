using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunAudio : MonoBehaviour
{
    [SerializeField]
    private AK.Wwise.Event shootEvent = null;
    [SerializeField]
    private AK.Wwise.RTPC chargeRTPC = null;

    public void PlayShoot(int damage)
    {
        chargeRTPC.SetValue(gameObject, damage);
        shootEvent.Post(gameObject);
    }
}
