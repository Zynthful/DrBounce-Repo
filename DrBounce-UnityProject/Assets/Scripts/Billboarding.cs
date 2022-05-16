using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboarding : MonoBehaviour
{
    private GameObject camTransform;

    void Start()
    {
        camTransform = FindObjectOfType<MouseLook>().gameObject;
    }

    void Update()
    {
        transform.rotation = camTransform.transform.rotation;
    }
}