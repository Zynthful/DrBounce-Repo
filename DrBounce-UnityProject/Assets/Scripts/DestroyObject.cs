using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyObject : MonoBehaviour
{
    public void Destroy(GameObject obj)
    {
        MonoBehaviour.Destroy(obj);
    }

    public void DestroyThis()
    {
        MonoBehaviour.Destroy(gameObject);
    }
}
