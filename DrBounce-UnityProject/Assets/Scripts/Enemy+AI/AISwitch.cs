using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AISwitch : MonoBehaviour
{
    // Start is called before the first frame update

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            foreach (EnemyHealth test in GetComponentsInChildren<EnemyHealth>())
            {
                if (test.gameObject.GetComponent<BouncyEnemy>())
                {
                    test.gameObject.GetComponent<BouncyEnemy>().enabled = true;
                }
                if (test.gameObject.GetComponent<NoBounceEnemy>())
                {
                    test.gameObject.GetComponent<NoBounceEnemy>().enabled = true;
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            foreach (EnemyHealth test in GetComponentsInChildren<EnemyHealth>())
            {
                if (test.gameObject.GetComponent<BouncyEnemy>())
                {
                    test.gameObject.GetComponent<BouncyEnemy>().enabled = false;
                }
                if (test.gameObject.GetComponent<NoBounceEnemy>())
                {
                    test.gameObject.GetComponent<NoBounceEnemy>().enabled = false;
                }
            }
        }
    }
}
