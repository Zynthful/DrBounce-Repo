using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPacks : MonoBehaviour
{
    private int amountHealed;

    private int amountOfBounces = 0;
    private int healModifier = 3;

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
