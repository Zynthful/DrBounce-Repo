using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WwiseEvent : MonoBehaviour
{
    [SerializeField]
    private AK.Wwise.Event @event = null;

    [SerializeField]
    private GameObject objToPost = null;

    public void Post()
    {
        @event.Post(objToPost);
    }

    public void Post(GameObject obj)
    {
        @event.Post(obj);
    }
}
