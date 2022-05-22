using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Bouncing : MonoBehaviour
{
    public enum BounceType
    {
        E_Back,
        E_Up,
        E_Away,
        W_Straight
    }

    public enum AxisDirection
    {
        GreenY,
        RedX,
        BlueZ,
        InverseGreen,
        InverseRed,
        InverseBlue
    }

    public BounceType bType;

    [SerializeField] float bounceForceMod = 14;

    [Header("Events")]
    [SerializeField]
    private UnityEvent onBounceAny = null;  // Invoked when any bounce occurs
    [SerializeField]
    private UnityEvent<int> onBouncePlayer = null; // Invoked when the player bounces off of it
    [SerializeField]
    private UnityEvent onBounceObject = null; // Invoked when an object other than the player bounces off of it

    [SerializeField]
    private float basePlayerKnockback = 15;

    public Vector3[] BounceBack(Vector3 position, Vector3 origin)
    {
        Vector3[] vectors = new Vector3[3];
        Vector3 dir = (origin - position).normalized;
        vectors[0] = position;
        vectors[1] = position;
        vectors[2] = new Vector3(dir.x, dir.y + .3f, dir.z) * bounceForceMod;
        return vectors;
    }

    public Vector3[] PlayerBounceBack(Vector3 dir)
    {
        Vector3[] vectors = new Vector3[1];
        dir = -dir;

        vectors[0] = new Vector3(dir.x, dir.y + .25f, dir.z) * bounceForceMod;
        return vectors;
    }

    public Vector3[] BounceUp(Transform collision, Vector3 position)
    {
        Vector3[] vectors = new Vector3[3];
        vectors[0] = new Vector3(collision.position.x, collision.position.y + (collision.localScale.y / 2), collision.position.z);
        vectors[1] = position;
        vectors[2] = Vector3.up * bounceForceMod;
        return vectors;
    }

    public Vector3[] PlayerBounceUp(float gravity)
    {
        Vector3[] vectors = new Vector3[1];
        vectors[0] = Vector3.up;

        vectors[0].y = Mathf.Sqrt(bounceForceMod * -2 * gravity);
        return vectors;
    }

    public Vector3[] BounceForward(Transform collision, Vector3 position, Vector3 origin)
    {
        Vector3[] vectors = new Vector3[3];
        Vector3 dir = (new Vector3(position.x, 0, position.z) - new Vector3(origin.x, 0, origin.z)).normalized;

        vectors[0] = new Vector3(collision.position.x + ((collision.localScale.x / 2) * dir.x), collision.position.y + (collision.localScale.y / 2), collision.position.z + ((collision.localScale.z / 2) * dir.z));
        dir = (new Vector3(vectors[0].x, 0, vectors[0].z) - new Vector3(origin.x, 0, origin.z)).normalized; dir.y = .2f;

        vectors[1] = transform.position;
        vectors[2] = new Vector3(dir.x, dir.y + .25f, dir.z) * bounceForceMod;

        return vectors;
    }

    public Vector3[] PlayerBounceForward(Transform collision, Vector3 position, Vector3 dir)
    {
        Vector3[] vectors = new Vector3[2];

        vectors[0] = new Vector3(collision.position.x + ((collision.localScale.x / 2) * dir.x), collision.position.y + (collision.localScale.y / 2), collision.position.z + ((collision.localScale.z / 2) * dir.z));

        dir = (new Vector3(vectors[0].x, 0, vectors[0].z) - new Vector3(position.x, 0, position.z)).normalized; dir.y = .2f;

        vectors[1] = new Vector3(dir.x, dir.y + .2f, dir.z) * bounceForceMod;

        return vectors;
    }

    public Vector3[] BounceStraight(Transform collision, Vector3 normal, Vector3 position, bool isPlayer, bool boostY)
    {
        Vector3[] vectors = new Vector3[3];

        vectors[0] = position;

        vectors[1] = position;

        vectors[2] = normal.normalized; 
        
        if(boostY) vectors[2].y += .2f;

        if (isPlayer)
        {
            PlayerMovement.instance.bounceForce = vectors[2] *= bounceForceMod;
            //PlayerMovement.instance.velocity += (vectors[2] * basePlayerKnockback);
        }
        else
        {

            vectors[2] *= bounceForceMod;
        }

        return vectors;
    }

    public Vector3[] BounceObject(Vector3 position, Vector3 direction, Collision collision, Vector3 origin, bool boostY = true)
    {
        onBounceAny?.Invoke();
        onBounceObject?.Invoke();

        switch (bType)
        {
            case BounceType.E_Back:
                return BounceBack(position, origin);

            case BounceType.E_Up:
                return BounceUp(collision.transform, position);

            case BounceType.E_Away:
                return BounceForward(collision.transform, position, origin);

            case BounceType.W_Straight:
                return BounceStraight(collision.transform, collision.transform.TransformDirection(Vector3.up), position, false, boostY);
        }

        return null;
    }

    public Vector3[] BouncePlayer(Vector3 position, Vector3 direction, ControllerColliderHit collision, int numOfBounces, bool boostY = true)
    {
        onBounceAny?.Invoke();
        onBouncePlayer?.Invoke(numOfBounces);

        switch (bType)
        {
            case BounceType.E_Back:
                return PlayerBounceBack(direction);

            case BounceType.E_Up:
                return PlayerBounceUp(GameManager.gravity);

            case BounceType.E_Away:
                return PlayerBounceForward(collision.transform, position, direction);

            case BounceType.W_Straight:
                return BounceStraight(collision.transform, collision.transform.TransformDirection(Vector3.up), position, true, boostY);
        }

        return null;
    }
}