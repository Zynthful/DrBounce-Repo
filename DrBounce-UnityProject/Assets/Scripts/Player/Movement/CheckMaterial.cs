// This script checks the material switch this object is colliding with, then sets the switch value on the given object.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckMaterial : MonoBehaviour
{
    [SerializeField]
    [Tooltip("The object to set the switch on.")]
    private GameObject switchObj = null;

    // Current/last material switch that this object is colliding/has collided with
    private AK.Wwise.Switch currentMaterial = null;

    private void OnTriggerEnter(Collider other)
    {
        MaterialSwitch materialSwitch = other.gameObject.GetComponent<MaterialSwitch>();
        if (materialSwitch != null)
        {
            if (currentMaterial != materialSwitch.GetMaterial() && materialSwitch.GetMaterial() != null)
            {
                currentMaterial = materialSwitch.GetMaterial();
                currentMaterial.SetValue(switchObj);
            }
        }
    }

    public AK.Wwise.Switch GetCurrentMaterial()
    {
        return currentMaterial;
    }
}
