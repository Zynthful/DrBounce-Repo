using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

        foreach(PhysicMaterial mat in physicMaterials)
        {
            mat.dynamicFriction = 0.2f; mat.bounciness = .2f;
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

        Vector3 newPos = transform.position;
        if ((dir.y < -BounceAwayAngleThreshold && collision.contacts[0].normal.normalized.y > 0) || (dir.y > BounceAwayAngleThreshold && collision.contacts[0].normal.normalized.y < 0))
        {
            newPos = new Vector3((2 * collision.transform.position.x) - (transform.position.x * Mathf.Abs(dir.x)), transform.position.y, (2 * collision.transform.position.z) - (transform.position.z * Mathf.Abs(dir.z)));
            dir.y = -dir.y;
        }
        else
        {
            newPos = new Vector3((2 * collision.transform.position.x) - (transform.position.x * Mathf.Abs(dir.x)), collision.transform.position.y + (collision.transform.localScale.y / 2), (2 * collision.transform.position.z) - (transform.position.z * Mathf.Abs(dir.z))); ;
        }

        transform.position = newPos;
        rb.velocity = new Vector3(dir.x, dir.y + .25f, dir.z) * forceMod;
    }

    void ResetScript()
    {
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
}
