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
    [Tooltip("NOTE: Only works when the gun is not held by the player (since its colliders are disabled).")]
    private bool lookForThrownGun = false;
    [SerializeField]
    private float triggerDelay = 0.0f;
    [SerializeField]
    private bool triggerOnce = false;

    private bool detected = false;
    private bool triggeredEnter = false;
    private bool triggeredExit = false;

    [Header("Events")]
    public UnityEvent<GameObject> onPreDetect = null;
    public UnityEvent<GameObject> onDetect = null;
    public UnityEvent<GameObject> onDetectStay = null;
    public UnityEvent onLostDetection = null;

    private void OnTriggerEnter(Collider other)
    {
        if (triggerOnce && triggeredEnter)
            return;

        if (IsInTrigger(other))
        {
            onPreDetect.Invoke(other.gameObject);
            StartCoroutine(DetectDelay(other.gameObject, other));
        }
    }

    private void OnTriggerStay(Collider other)
    {
        // Stop if we're triggering only once and we've already triggered
        if (triggerOnce && triggeredExit)
            return;

        // Stop if we've not the delay
        if (!detected)
            return;

        // Check the object we're detecting is the object we're looking for
        if (IsInTrigger(other))
        {
            onDetectStay.Invoke(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (triggerOnce && triggeredExit)
            return;

        if (IsInTrigger(other))
        {
            LostDetection();
        }
    }

    private bool IsInTrigger(Collider trigger)
    {
        // wtf
        return ((lookForLayer.value & (1 << trigger.gameObject.layer)) > 0) || lookForThrownGun && trigger.gameObject.GetComponent<GunThrowing>();
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
        triggeredEnter = true;
        detected = true;
        onDetect.Invoke(obj);
    }

    private void LostDetection()
    {
        triggeredExit = true;
        detected = false;
        onLostDetection.Invoke();
    }
}