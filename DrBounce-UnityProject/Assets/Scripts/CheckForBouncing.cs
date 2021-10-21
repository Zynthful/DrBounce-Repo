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

    private void Start()
    {
        if (GetComponent<GunThrowing>())
        {
            specialInteractions = GetComponent<GunThrowing>();
        }

        rb = GetComponent<Rigidbody>();
    }

    protected void OnCollisionEnter(Collision collision)
    {
        if (CanBounce() && GameManager.bounceableLayers == (GameManager.bounceableLayers | 1 << collision.gameObject.layer))
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
                    returnVectors = b.BounceForward(collision, transform.position, bounceOriginPoint);
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
}
