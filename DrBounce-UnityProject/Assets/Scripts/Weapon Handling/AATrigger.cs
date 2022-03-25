using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AATrigger : MonoBehaviour
{
    private void OnBecameInvisible()
    {
        AAManager.RemoveTransform(this.transform);
    }

    private void OnBecameVisible()
    {
        AAManager.AddTransform(this.transform);
    }

    private void OnDisable()
    {
        AAManager.RemoveTransform(this.transform);
    }
}
