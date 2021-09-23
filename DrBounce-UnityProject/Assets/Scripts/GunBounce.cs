﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;
public class GunBounce : MonoBehaviour
{
    [SerializeField] bool returning; 
    [SerializeField] float forceMod;
    [SerializeField] bool canThrow;
    [SerializeField] string[] bounceableTags;
    [SerializeField] Transform weaponHolderTransform = null;
    Vector3 handPosition;
    Vector3 originPoint;
    Rigidbody rb;

    [Header("Feedbacks")]
    public MMFeedbacks BounceFeedback;
    public MMFeedbacks CatchFeedback;
    public MMFeedbacks BounceHitFeedback;

    // Start is called before the first frame update
    void Start()
    {
        handPosition = transform.localPosition;
        canThrow = true;
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeAll;
        transform.parent = weaponHolderTransform;
        transform.rotation = Quaternion.identity;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && canThrow)
        {
            canThrow = false;
            Thrown(transform.position);
        }

        if (Input.GetKeyDown(KeyCode.Mouse2))
        {
            ResetScript();
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (transform.parent)
        {
            transform.rotation = weaponHolderTransform.rotation;
        }
        if(returning)
        {
            returning = false;
            Vector3 dir = (originPoint - transform.position).normalized;
            rb.velocity = new Vector3(dir.x, .3f, dir.z) * forceMod; 
        }
    }

    public void Thrown(Vector3 position)
    {
        //when we get out of prototype we need to made the world model seperate from the fp model
        gameObject.layer = 0;
        foreach (Transform child in transform) 
            child.gameObject.layer = 0;

        returning = false;
        rb.constraints = RigidbodyConstraints.None;
        transform.parent = null;
        originPoint = position;
        Vector3 dir = transform.forward;
        rb.velocity = new Vector3(dir.x, dir.y + .1f, dir.z) * forceMod;
    }

    void ResetScript()
    {
        gameObject.layer = 7;
        foreach (Transform child in transform)
            child.gameObject.layer = 7;

        returning = false;
        rb.velocity = Vector3.zero;
        rb.constraints = RigidbodyConstraints.FreezeAll;
        transform.parent = weaponHolderTransform;
        transform.localPosition = handPosition;
        transform.rotation = weaponHolderTransform.rotation;
        canThrow = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        foreach(string tag in bounceableTags)
        {
            if (collision.transform.CompareTag(tag))
            {
                Debug.Log("Collided");
                BounceFeedback?.PlayFeedbacks();
                collision.transform.GetComponentInChildren<MMFeedbacks>().PlayFeedbacks();
                rb.velocity = Vector3.zero;
                returning = true;
            }
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !transform.parent)
        {
            CatchFeedback?.PlayFeedbacks();
            ResetScript();
        }
    }
}
