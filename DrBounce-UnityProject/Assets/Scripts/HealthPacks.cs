using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPacks : MonoBehaviour
{
    private int amountHealed = 50;

    private bool hasBounced = false;
    private int healModifier = 2;


    private bool pickupable;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        pickupable = true;
    }

    private void OnTriggerExit(Collider other)
    {
        pickupable = false;
    }
}
