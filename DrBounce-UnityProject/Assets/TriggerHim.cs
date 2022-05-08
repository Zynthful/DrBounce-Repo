using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerHim : MonoBehaviour
{

    [SerializeField] MeshRenderer vent;
    [SerializeField] GameObject him;

    private void OnTriggerEnter(Collider other) 
    {
        if(other.transform.root == PlayerMovement.player && GameManager.s_Instance.triggerThatThing)
        {
            GameManager.s_Instance.triggerThatThing = false;
            vent.enabled = false;
            him.SetActive(true);
            him.GetComponent<Animator>().SetTrigger("GOGOGO");
        }
    }
}
