// This script checks the material switch this object is colliding with, then sets the switch value on the given object.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckMaterial : MonoBehaviour
{
    [Header("CheckMaterial Settings")]
    [SerializeField]
    [Tooltip("The object to set the switch on.")]
    private GameObject switchObj = null;

    [SerializeField]
    private LayerMask layerMask;

    // Current/last material switch that this object is colliding/has collided with
    private AK.Wwise.Switch currentMaterial = null;

    public void Check(Vector3 centre, Vector3 halfExtents, Vector3 direction, Quaternion orientation, float distance)
    {
        RaycastHit[] hits = Physics.BoxCastAll(centre, halfExtents, direction, orientation, distance, ~layerMask);
        MaterialSwitch[] mats = new MaterialSwitch[hits.Length];
        int indexOfHighestPriority = -1;
        for (int i = 0; i < hits.Length; i++)
        {
            mats[i] = hits[i].collider.gameObject.GetComponent<MaterialSwitch>();
            if (mats[i] != null)
            {
                // Compare priority
                if (indexOfHighestPriority == -1 || mats[i].GetPriority() > mats[indexOfHighestPriority].GetPriority())
                {
                    indexOfHighestPriority = i;
                }
            }
        }
        // Check if we found a material
        if (indexOfHighestPriority != -1)
        {
            SetMaterial(mats[indexOfHighestPriority].GetMaterial());
        }
    }

    public void SetMaterial(AK.Wwise.Switch material)
    {
        if (currentMaterial != material)
        {
            currentMaterial = material;
            currentMaterial.SetValue(switchObj);
        }
    }

    public AK.Wwise.Switch GetCurrentMaterial() { return currentMaterial; }
}
