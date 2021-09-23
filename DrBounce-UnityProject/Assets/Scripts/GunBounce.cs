using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;
public class GunBounce : MonoBehaviour
{
    [SerializeField] bool returning; 
    [SerializeField] float forceMod;
    [SerializeField] bool canThrow;
    [SerializeField] string[] bounceableTags;
    Camera cam;
    Vector3 handPosition;
    Vector3 originPoint;
    Rigidbody rb;

    [Header("Feedbacks")]
    public MMFeedbacks BounceFeedback;
    public MMFeedbacks CatchFeedback;
    public MMFeedbacks BounceHitFeedback;

    private bool inFlight;

    //event for sending amount of bounces to the shooting script
    public delegate void PickUp();
    public static event PickUp OnPickUp;

    //event for the gun hits the floor, causing the gun ti lose it's charge
    public delegate void GunDropped();
    public static event GunDropped OnFloorCollision;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        handPosition = transform.localPosition;
        canThrow = true;
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeAll;
        transform.parent = cam.transform;
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
            transform.rotation = cam.transform.rotation;
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
        transform.parent = cam.transform;
        transform.localPosition = handPosition;
        transform.rotation = cam.transform.rotation;
        canThrow = true;
        inFlight = false;
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
                inFlight = true;
            }
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !transform.parent) 
        {
            if (inFlight) { OnPickUp?.Invoke(); }
            CatchFeedback?.PlayFeedbacks();
            ResetScript();
        }

        //occures when the gun hits the floor, removing charge from the gun
        if (other.gameObject.layer == 6) 
        {
            OnFloorCollision?.Invoke();
            inFlight = false;
        }
        
    }
}
