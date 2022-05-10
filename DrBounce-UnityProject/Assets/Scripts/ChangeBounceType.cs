using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeBounceType : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(changeBounceType());
    }

    private IEnumerator changeBounceType()
    {
        yield return new WaitForSeconds(0.2f);
        GetComponent<Bouncing>().bType = Bouncing.BounceType.E_Back;
    }
}
