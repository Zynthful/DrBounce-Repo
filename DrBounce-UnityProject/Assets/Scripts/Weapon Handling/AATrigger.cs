using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AATrigger : MonoBehaviour
{
    private void OnBecameInvisible()
    {
        AAManager.RemoveTransform(this.transform.parent.parent);
    }

    private void OnBecameVisible()
    {
        AAManager.AddTransform(this.transform.parent.parent);
    }

    private void OnDisable()
    {
        AAManager.RemoveTransform(this.transform.parent.parent);
    }
}
