using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    void OnEnable()
    {
        SkipCutscene.OnEnd += SetRotation;
    }


    void OnDisable()
    {
        SkipCutscene.OnEnd -= SetRotation;
    }

    private void SetRotation() 
    {
        this.gameObject.transform.Rotate(0, -22, 0, Space.Self);
    }
}
