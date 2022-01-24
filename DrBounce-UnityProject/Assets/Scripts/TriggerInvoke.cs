using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerInvoke : MonoBehaviour
{
    [Header("Trigger Settings")]
    [SerializeField]
    private LayerMask lookForLayer = new LayerMask();
    [SerializeField]
    private float triggerDelay = 0.0f;

    private bool detected = false;

    [Header("Events")]
    public UnityEvent<GameObject> onPreDetect = null;
    public UnityEvent<GameObject> onDetect = null;
    public UnityEvent<GameObject> onDetectStay = null;
    public UnityEvent onLostDetection = null;

    private void OnTriggerEnter(Collider other)
    {
        if (IsInTrigger(other))
        {
            onPreDetect.Invoke(other.gameObject);
            StartCoroutine(DetectDelay(other.gameObject, other));
        }
    }

    private void OnTriggerStay(Collider other)
    {
        // Check the object we're detecting is the object we're looking for, and that we've passed the delay
        if (IsInTrigger(other) && detected)
        {
            onDetectStay.Invoke(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (IsInTrigger(other))
        {
            LostDetection();
        }
    }

    private bool IsInTrigger(Collider trigger)
    {
        // wtf
        return (lookForLayer.value & (1 << trigger.gameObject.layer)) > 0;
    }

    // cringe
    private IEnumerator DetectDelay(GameObject obj, Collider trigger)
    {
        yield return new WaitForSeconds(triggerDelay);
        if (IsInTrigger(trigger))
        {
            Detect(obj);
        }
    }

    private void Detect(GameObject obj)
    {
        detected = true;
        onDetect.Invoke(obj);
    }

    private void LostDetection()
    {
        detected = false;
        onLostDetection.Invoke();
    }
}