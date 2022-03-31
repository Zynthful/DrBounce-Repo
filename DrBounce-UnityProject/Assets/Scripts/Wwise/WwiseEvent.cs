using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WwiseEvent : MonoBehaviour
{
    [SerializeField]
    protected AK.Wwise.Event @event = null;

    [SerializeField]
    protected GameObject objToPost = null;

    [SerializeField]
    protected PostOptions postOn = PostOptions.None;

    protected enum PostOptions
    {
        None,
        Start,
        Awake,
        OnEnable,
        OnDisable,
        OnDestroy,
    }

    protected virtual void Awake()
    {
        if (postOn == PostOptions.Awake)
        {
            Post();
        }
    }

    protected virtual void OnEnable()
    {
        if (postOn == PostOptions.OnEnable)
        {
            Post();
        }
    }

    protected virtual void Start()
    {
        if (postOn == PostOptions.Start)
        {
            Post();
        }
    }

    protected virtual void OnDisable()
    {
        if (postOn == PostOptions.OnDisable)
        {
            Post();
        }
    }

    protected virtual void OnDestroy()
    {
        if (postOn == PostOptions.OnDestroy)
        {
            Post();
        }
    }


    public virtual void Post()
    {
        @event.Post(objToPost);
    }

    public virtual void Post(GameObject obj)
    {
        @event.Post(obj);
    }
}
