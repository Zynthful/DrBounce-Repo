using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bouncing : MonoBehaviour
{

    public enum BounceType
    {
        Back,
        Up,
        Away,
        Left,
        Right,
    }

    [SerializeField] float bounceForceMod;
    [SerializeField] [Range(0.01f, 1f)] float BounceAwayAngleThreshold;
    public BounceType bType;

    public Vector3[] BounceBack(Vector3 position, Vector3 origin)
    {

        Vector3[] vectors = new Vector3[3];
        Vector3 dir = (origin - position).normalized;
        vectors[0] = position;
        vectors[1] = position;
        vectors[2] = new Vector3(dir.x, dir.y + .3f, dir.z) * bounceForceMod;
        return vectors;
    }

    public Vector3[] BounceUp(Transform enemyTransform, Vector3 position)
    {
        Vector3[] vectors = new Vector3[3];
        vectors[0] = new Vector3(enemyTransform.position.x, enemyTransform.position.y + (enemyTransform.localScale.y / 2), enemyTransform.position.z);
        vectors[1] = position;
        vectors[2] = Vector3.up * bounceForceMod;
        return vectors;
    }

    public Vector3[] BounceForward(Collision collision, Vector3 position, Vector3 origin)
    {
        Vector3[] vectors = new Vector3[3];
        Vector3 dir = (position - origin).normalized;

        vectors[0] = position;
        if ((dir.y < -BounceAwayAngleThreshold && collision.contacts[0].normal.normalized.y > 0) || (dir.y > BounceAwayAngleThreshold && collision.contacts[0].normal.normalized.y < 0))
        {
            Debug.Log("Top/Bottom bounce");
            vectors[0] = new Vector3((2 * collision.transform.position.x) - position.x, position.y, (2 * collision.transform.position.z) - position.z);
            dir.y = -dir.y - .5f;
        }
        else
        {
            vectors[0] = new Vector3(collision.transform.position.x + ((collision.transform.localScale.x / 2) * dir.x), collision.transform.position.y + (collision.transform.localScale.y / 2), collision.transform.position.z + ((collision.transform.localScale.z / 2) * dir.z));
            dir.y = .2f;
        }

        vectors[1] = transform.position;
        vectors[2] = new Vector3(dir.x, dir.y + .25f, dir.z) * bounceForceMod;

        return vectors;
    }
}
