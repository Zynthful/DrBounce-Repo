using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetScale : MonoBehaviour
{
    [SerializeField]
    private Transform target = null;

    public void SetTargetScale(Vector3 value)
    {
        target.localScale = value;
    }
    public void SetTargetScaleX(float value)
    {
        target.localScale = new Vector3(value, target.localScale.y, target.localScale.z);
    }
    public void SetTargetScaleY(float value)
    {
        target.localScale = new Vector3(target.localScale.x, value, target.localScale.z);
    }
    public void SetTargetScaleZ(float value)
    {
        target.localScale = new Vector3(target.localScale.x, target.localScale.y, value);
    }
    public void SetTargetScaleXY(float value)
    {
        target.localScale = new Vector3(value, value, target.localScale.z);
    }
}