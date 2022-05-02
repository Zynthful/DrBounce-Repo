using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunAudioVisualiser : MonoBehaviour
{
    [SerializeField]
    private MeshRenderer gunRenderer = null;

    [SerializeField]
    private AK.Wwise.RTPC volumeRTPC = null;

    private void Update()
    {
        gunRenderer.sharedMaterials[4].SetFloat("_visualiser", volumeRTPC.GetGlobalValue());
    }
}
