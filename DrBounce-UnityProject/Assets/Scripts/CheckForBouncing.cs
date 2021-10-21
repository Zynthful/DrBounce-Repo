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
        }
    }

    protected void OnCollisionEnter(Collision collision)
    {
        if (CanBounce() && GameManager.s_Instance.bounceableLayers == (GameManager.s_Instance.bounceableLayers | 1 << collision.gameObject.layer))
        {
            if (collision.gameObject.GetComponent<Enemy>())
            {
                EnemyAudio audio = collision.gameObject.GetComponent<Enemy>()?.enemyAudio;
                audio?.PlayBounce();
            }

            Vector3[] returnVectors = new Vector3[2];

            Bouncing b = collision.gameObject.GetComponent<Bouncing>();
            switch (b.bType)
            {
                case Bouncing.BounceType.E_Back:
                    returnVectors = b.BounceBack(transform.position, bounceOriginPoint);
                    break;

                case Bouncing.BounceType.E_Up:
                    returnVectors = b.BounceUp(collision.transform, transform.position);
                    break;

                case Bouncing.BounceType.E_Away:
                    returnVectors = b.BounceForward(collision.transform, transform.position, bounceOriginPoint);
                    break;

                case Bouncing.BounceType.W_Straight:
                    returnVectors = b.BounceBack(transform.position, bounceOriginPoint);
                    break;
            }
            transform.position = returnVectors[0];
            bounceOriginPoint = returnVectors[1];
            rb.velocity = returnVectors[2];

            specialInteractions.Bounced(collision);
        }
    }

    protected void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (CanBounce() && GameManager.s_Instance.bounceableLayers == (GameManager.s_Instance.bounceableLayers | 1 << hit.gameObject.layer) && hit != recentHit)
        {
            if (hit.gameObject.GetComponent<Enemy>())
            {
                EnemyAudio audio = hit.gameObject.GetComponent<Enemy>()?.enemyAudio;
                audio?.PlayBounce();
            }

            Vector3[] returnVectors = new Vector3[2];

            Bouncing b = hit.gameObject.GetComponent<Bouncing>();
            switch (b.bType)
            {
                case Bouncing.BounceType.E_Back:
                    returnVectors = b.BounceBack(transform.position, bounceOriginPoint);
                    break;

                case Bouncing.BounceType.E_Up:
                    returnVectors = b.BounceUp(hit.transform, transform.position);
                    break;

                case Bouncing.BounceType.E_Away:
                    returnVectors = b.BounceForward(hit.transform, transform.position, bounceOriginPoint);
                    break;

                case Bouncing.BounceType.W_Straight:
                    returnVectors = b.BounceBack(transform.position, bounceOriginPoint);
                    break;
            }
            transform.position = returnVectors[0];
            bounceOriginPoint = returnVectors[1];
            cc.velocity.Set(returnVectors[2].x, returnVectors[2].y, returnVectors[2].z);

            recentHit = hit;

            if(recentHitRun != null)
                StopCoroutine(recentHitRun);
            recentHitRun = StartCoroutine(RecentBounce(.02f));
        }
    }

    public void ObjectThrown()
    {
        bounceOriginPoint = transform.position;
    }

    public bool CanBounce()
    {
        foreach (RequirementsForBounce.Requirements req in bounceRequirements.requirements)
        {
            if(!HandleReq(req))
            {
                return false;
            }
        }

        return true;
    }

    private bool HandleReq(RequirementsForBounce.Requirements req)
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
        }
        return false;
    }

    IEnumerator RecentBounce(float time)
    {
        yield return new WaitForSeconds(time);
        recentHit = null;
    }
}
