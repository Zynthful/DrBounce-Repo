using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckForBouncing : MonoBehaviour
{
    [SerializeField] RequirementsForBounce bounceRequirements;

    Vector3 bounceOriginPoint;
    Vector3 newVelocity;

    GunThrowing specialInteractions;
    Rigidbody rb;
    CharacterController cc;
    PlayerMovement pm;
    ControllerColliderHit recentHit;
    Coroutine recentHitRun;

    private void Start()
    {
        if (GetComponent<GunThrowing>())
        {
            specialInteractions = GetComponent<GunThrowing>();
        }

        rb = GetComponent<Rigidbody>();

        if (!rb)
        {
            cc = GetComponent<CharacterController>();
            pm = GetComponent<PlayerMovement>();
        }
    }

    protected void OnCollisionEnter(Collision collision)
    {
        if (CanBounce(collision.gameObject) && GameManager.s_Instance.bounceableLayers == (GameManager.s_Instance.bounceableLayers | 1 << collision.gameObject.layer))
        {
            if (collision.gameObject.GetComponent<Enemy>())
            {
                EnemyAudio audio = collision.gameObject.GetComponent<Enemy>()?.enemyAudio;
                audio?.PlayBounce();
            }

            Vector3[] returnVectors = new Vector3[3];

            returnVectors = collision.gameObject.GetComponent<Bouncing>().BounceObject(transform.position, rb.velocity.normalized, collision.transform, bounceOriginPoint);
            if (returnVectors.Length > 0)
            {
                specialInteractions.Bounced(collision);

                transform.position = returnVectors[0];
                bounceOriginPoint = returnVectors[1];
                rb.velocity = returnVectors[2];
            }
        }
    }

    protected void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (CanBounce(hit.gameObject) && GameManager.s_Instance.bounceableLayers == (GameManager.s_Instance.bounceableLayers | 1 << hit.gameObject.layer) && hit != recentHit)
        {
            if (hit.gameObject.GetComponent<Enemy>())
            {
                EnemyAudio audio = hit.gameObject.GetComponent<Enemy>()?.enemyAudio;
                audio?.PlayBounce();
            }

            Vector3[] returnVectors = new Vector3[1];

            returnVectors = hit.gameObject.GetComponent<Bouncing>().BouncePlayer(transform.position, cc.velocity.normalized, hit.transform);
            if (returnVectors.Length > 0)
            {
                if (returnVectors.Length == 2)
                {
                    transform.position = returnVectors[0];
                    pm.velocity = returnVectors[1];
                }
                else
                {
                    pm.velocity = returnVectors[0];
                }
            }

            recentHit = hit;

            if(recentHitRun != null)
                StopCoroutine(recentHitRun);
            recentHitRun = StartCoroutine(RecentBounce(.175f));
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

        return true;
    }

    private bool HandleReq(RequirementsForBounce.Requirements req, GameObject other)
    {
        switch (req)
        {
            case RequirementsForBounce.Requirements.noParent:
                if (!transform.parent)
                    return true;
                break;

            case RequirementsForBounce.Requirements.amDead:
                if (GetComponent<Health>())
                {
                    if (GetComponent<Health>().ReturnDead())
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
                    if (!GetComponent<Health>().ReturnDead())
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
                else
                {
                    return false;
                }

            case RequirementsForBounce.Requirements.dontBounceAgainstEnemies:
                if (!other.GetComponent<Enemy>())
                {
                    return true;
                }
                break;
        }
        return false;
    }

    IEnumerator RecentBounce(float time)
    {
        yield return new WaitForSeconds(time);
        recentHit = null;
    }
}
