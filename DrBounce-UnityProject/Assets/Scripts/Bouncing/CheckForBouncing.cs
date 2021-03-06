using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckForBouncing : MonoBehaviour
{
    [SerializeField]
    private RequirementsForBounce bounceRequirements;

    Vector3 bounceOriginPoint;
    Vector3 newVelocity;

    GunThrowing specialInteractions;
    Rigidbody rb;
    CharacterController cc;
    PlayerMovement pm;
    ControllerColliderHit recentHit;
    Coroutine recentHitRun;

    bool isExplosive;

    private int numOfPlayerBounces = 0;

    [Tooltip("Only used if specific bounces is a requirement")]
    [SerializeField] private List<GameObject> exceptions;

    private GameObject lastBounced = null;  // Object that this has bounced off of last
    private float timeSinceLastBounce = 0.0f;

    private void Start()
    {
        if (GetComponent<GunThrowing>())
        {
            specialInteractions = GetComponent<GunThrowing>();
        }

        if (GetComponent<ExplosiveShot>())
            isExplosive = true;

        rb = GetComponent<Rigidbody>();

        if (!rb)
        {
            cc = GetComponent<CharacterController>();
            pm = GetComponent<PlayerMovement>();
        }

        if(!cc && !rb)
        {
            rb = GetComponentInParent<Rigidbody>();
            if (!rb)
                rb = GetComponentInChildren<Rigidbody>();
        }

        recentHit = null;
    }

    protected void OnCollisionEnter(Collision collision)
    {
        if (CanBounce(collision.gameObject) && GameManager.s_Instance.bounceableLayers == (GameManager.s_Instance.bounceableLayers | 1 << collision.gameObject.layer))
        {
            Vector3[] returnVectors = new Vector3[3];

            if (isExplosive)
                returnVectors = collision.gameObject.GetComponent<Bouncing>().BounceObject(transform.position, rb.velocity.normalized, collision, bounceOriginPoint, false);
            else
                returnVectors = collision.gameObject.GetComponent<Bouncing>().BounceObject(transform.position, rb.velocity.normalized, collision, bounceOriginPoint);

            if (returnVectors.Length > 0)
            {
                if (specialInteractions)
                {

                    if (collision.gameObject == lastBounced)
                    {
                        specialInteractions.Bounced(collision);
                    }
                    else
                    {
                        specialInteractions.BouncedUnique(collision);
                    }
                }

                lastBounced = collision.gameObject;

                transform.position = returnVectors[0];
                bounceOriginPoint = returnVectors[1];

                if (isExplosive)
                    rb.velocity = returnVectors[2].normalized * rb.velocity.magnitude;
                else
                    rb.velocity = returnVectors[2];
            }
        }
    }

    protected void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (CanBounce(hit.gameObject) && GameManager.s_Instance.bounceableLayers == (GameManager.s_Instance.bounceableLayers | 1 << hit.gameObject.layer) && hit != recentHit)
        {
            Vector3[] returnVectors = new Vector3[1];

            SetNumOfPlayerBounces(numOfPlayerBounces + 1);
            returnVectors = hit.gameObject.GetComponent<Bouncing>().BouncePlayer(transform.position, cc.velocity.normalized, hit, numOfPlayerBounces);
            if (returnVectors.Length > 0)
            {
                if (returnVectors.Length == 2)
                {
                    transform.position = returnVectors[0];
                    pm.velocity = returnVectors[1];
                }
                else if(returnVectors.Length == 3)
                {
                    transform.position = returnVectors[0];
                    pm.velocity = returnVectors[2];
                }
                else
                {
                    pm.velocity = returnVectors[0];
                }
            }

            recentHit = hit;

            if(recentHitRun != null)
                StopCoroutine(recentHitRun);
            recentHitRun = StartCoroutine(RecentBounce(.25f));
        }
    }

    public void ObjectThrown()
    {
        bounceOriginPoint = transform.position;
    }

    public bool CanBounce(GameObject other)
    {
        foreach (RequirementsForBounce.Requirements req in bounceRequirements.requirements)
        {
            if(!HandleReq(req, other))
            {
                return false;
            }
        }

        if (other.GetComponent<EnemyHealth>())
            other.GetComponent<EnemyHealth>().SetRecentAttacked();
        return true;
    }

    private bool HandleReq(RequirementsForBounce.Requirements req, GameObject other)
    {
        if(exceptions.Contains(other))
            return true;
        
        switch (req)
        {
            case RequirementsForBounce.Requirements.noParent:
                if (!transform.parent)
                    return true;
                break;

            case RequirementsForBounce.Requirements.amDead:
                if (GetComponent<Health>())
                {
                    if (GetComponent<Health>().GetIsDead())
                        return true;
                }
                else
                {
                    return !gameObject.activeSelf;
                }
                break;

            case RequirementsForBounce.Requirements.amAlive:
                if (GetComponent<Health>())
                {
                    if (!GetComponent<Health>().GetIsDead())
                        return true;
                }
                else
                {
                    return gameObject.activeSelf;
                }
                break;

            case RequirementsForBounce.Requirements.onlyBounceAgainstEnemies:
                if (other.GetComponent<Enemy>())
                {
                    return true;
                }
                return false;
    
            case RequirementsForBounce.Requirements.dontBounceAgainstEnemies:
                if (!other.GetComponent<Enemy>())
                {
                    return true;
                }
                return false;
        }
        return false;
    }

    IEnumerator RecentBounce(float time)
    {
        yield return new WaitForSeconds(time);
        recentHit = null;
        recentHitRun = null;
    }

    public void SetNumOfPlayerBounces(int value)
    {
        numOfPlayerBounces = value;
    }

    public int GetNumOfPlayerBounces()
    {
        return numOfPlayerBounces;
    }
}