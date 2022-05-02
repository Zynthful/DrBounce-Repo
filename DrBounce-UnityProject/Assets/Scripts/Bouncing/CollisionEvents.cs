using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CollisionEvents : MonoBehaviour
{
    [Header("Collision Event Trigger Settings")]
    [SerializeField]
    private LayerMask layerMask = new LayerMask();

    [Header("Events")]
    [SerializeField]
    private UnityEvent<float> onCollisionEnter = null;      // Passes collision impulse magnitude
    [SerializeField]
    private UnityEvent onCollisionExit = null;
    [SerializeField]
    private UnityEvent onCollisionStay = null;

    private void OnCollisionEnter(Collision collision)
    {
        if (IsMatchingLayer(collision.gameObject, layerMask))
        {
            onCollisionEnter?.Invoke(collision.impulse.magnitude);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (IsMatchingLayer(collision.gameObject, layerMask))
        {
            onCollisionExit?.Invoke();
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (IsMatchingLayer(collision.gameObject, layerMask))
        {
            onCollisionStay?.Invoke();
        }
    }

    private bool IsMatchingLayer(GameObject obj, LayerMask layer)
    {
        return (layer.value & (1 << obj.layer)) > 0;
    }
}