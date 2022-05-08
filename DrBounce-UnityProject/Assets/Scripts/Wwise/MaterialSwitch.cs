using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialSwitch : MonoBehaviour
{
    [SerializeField]
    private AK.Wwise.Switch material = null;
    public AK.Wwise.Switch GetMaterial() { return material; }
    public void SetMaterial(AK.Wwise.Switch value) { material = value; }

    [SerializeField]
    private int priority = 0;
    public int GetPriority() { return priority; }
}
