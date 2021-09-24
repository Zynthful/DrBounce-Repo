﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using MoreMountains.Feedbacks;
public class GunBounce : MonoBehaviour
{
    [SerializeField] bool returning; 
    [SerializeField] float forceMod;
    [SerializeField] bool canThrow;
    [SerializeField] LayerMask bounceableLayers;
    [SerializeField] Transform weaponHolderTransform = null;
    [SerializeField] [Range(0.01f, 1f)] float BounceAwayAngleThreshold;
    [SerializeField] [Range(0.6f, 2.5f)] float sideBounceAngleThreshold;
    List<PhysicMaterial> physicMaterials = new List<PhysicMaterial> { };
    bool throwGunDelay;
    Vector3 handPosition;
    Vector3 originPoint;
    Rigidbody rb;

    public InputMaster controls;

    [Header("Feedbacks")]
    public MMFeedbacks BounceFeedback;
    public MMFeedbacks CatchFeedback;
    public MMFeedbacks BounceHitFeedback;

    private bool inFlight;

    //event for sending amount of bounces to the shooting script
    public delegate void PickUp();
    public static event PickUp OnPickUp;

    //event for the gun hits the floor, causing the gun to lose it's charge
    public delegate void GunDropped();
    public static event GunDropped OnFloorCollision;

    // Start is called before the first frame update
    void Start()
    {
        handPosition = transform.localPosition;
        canThrow = true;
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeAll;
        transform.parent = weaponHolderTransform;
        transform.rotation = Quaternion.identity;

        Collider[] colliders = GetComponentsInChildren<Collider>();
        foreach(Collider col in colliders)
        {
            physicMaterials.Add(col.material);
        }
    }

    private void Awake()
    {
        controls = new InputMaster();
        controls.Player.RecallGun.performed += _ => ResetScript();
        controls.Player.ThrowGun.performed += _ => throwGunDelay = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (transform.parent)
        {
            transform.rotation = weaponHolderTransform.rotation;
        }

        if (throwGunDelay)
        {
            throwGunDelay = false;
            Thrown();
        }
    }

    public void Thrown()
    {
        if (canThrow)
        {
            canThrow = false;
            
            //when we get out of prototype we need to made the world model seperate from the fp model
            gameObject.layer = 0;
            foreach (Transform child in transform)
                child.gameObject.layer = 0;

            returning = false;
            rb.constraints = RigidbodyConstraints.None;
            transform.parent = null;
            originPoint = transform.position;
            Vector3 dir = transform.forward;
            rb.velocity = new Vector3(dir.x, dir.y + .1f, dir.z) * forceMod;

            foreach (PhysicMaterial mat in physicMaterials)
            {
                mat.dynamicFriction = 0.2f; mat.bounciness = .2f;
            }
        }
    }

    void BounceBack()
    {
        Vector3 dir = (originPoint - transform.position).normalized;
        rb.velocity = new Vector3(dir.x, dir.y + .3f, dir.z) * forceMod;
    }

    void BounceUp(Transform enemyTransform)
    {
        transform.position = new Vector3(enemyTransform.position.x, enemyTransform.position.y + (enemyTransform.localScale.y / 2), enemyTransform.position.z);
        rb.velocity = Vector3.up * forceMod;
    }

    void BounceForward(Collision collision)
    {
        Vector3 dir = (transform.position - originPoint).normalized;


        Debug.Log(dir.y + "  " + collision.contacts[0].normal.normalized.y);
        Vector3 newPos = transform.position;
        if ((dir.y < -BounceAwayAngleThreshold && collision.contacts[0].normal.normalized.y > 0) || (dir.y > BounceAwayAngleThreshold && collision.contacts[0].normal.normalized.y < 0))
        {
            newPos = new Vector3((2 * collision.transform.position.x) - transform.position.x, transform.position.y, (2 * collision.transform.position.z) - transform.position.z);
            dir.y = -dir.y - .5f;
        }
        else
        {
            newPos = new Vector3(collision.transform.position.x + ((collision.transform.localScale.x / 2) * dir.x), collision.transform.position.y + (collision.transform.localScale.y / 2), collision.transform.position.z + ((collision.transform.localScale.z / 2) * dir.z));
        }

        transform.position = newPos;
        originPoint = newPos;
        rb.velocity = new Vector3(dir.x, dir.y + .25f, dir.z) * forceMod;
    }

    void ResetScript()
    {
        if(!transform.parent)
            throwGunDelay = false;

        gameObject.layer = 7;
        foreach (Transform child in transform)
            child.gameObject.layer = 7;

        returning = false;
        canThrow = true;
        inFlight = false;

        rb.velocity = Vector3.zero;
        rb.constraints = RigidbodyConstraints.FreezeAll;
        transform.parent = weaponHolderTransform;
        transform.localPosition = handPosition;
        transform.rotation = weaponHolderTransform.rotation;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!transform.parent && bounceableLayers == (bounceableLayers | 1 << collision.gameObject.layer))
        {
            returning = true;
            inFlight = true;

            BounceFeedback?.PlayFeedbacks();
            collision.transform.GetComponentInChildren<MMFeedbacks>().PlayFeedbacks();

            switch (collision.gameObject.GetComponent<Enemy>().eType)
            {
                case Enemy.EnemyTypes.BlueBack:
                    BounceBack();
                    return;

                case Enemy.EnemyTypes.YellowUp:
                    BounceUp(collision.transform);
                    return;

                case Enemy.EnemyTypes.RedForward:
                    BounceForward(collision);
                    return;
            }
        }
        //occures when the gun hits the floor or a relatively flat surface, removing charge from the gun
        else if (collision.contacts[0].normal.normalized.y > .80f)
        {
            foreach (PhysicMaterial mat in physicMaterials)
            {
                mat.dynamicFriction = 0.85f; mat.bounciness = 0;
            }
            returning = true;
            OnFloorCollision?.Invoke();
            inFlight = false;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && !transform.parent && returning) 
        {
            if (inFlight) { OnPickUp?.Invoke(); }
            CatchFeedback?.PlayFeedbacks();
            ResetScript();
        }
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Player.ThrowGun.performed -= _ => Thrown();
        controls.Player.RecallGun.performed -= _ => ResetScript();
        controls.Disable();
    }
}
