using System.Collections;
using System.Collections.Generic;
using UnityEngine;

 #if UNITY_EDITOR
 using UnityEditor;
 #endif

public class Bouncing : MonoBehaviour
{

    public enum BounceType
    {
        E_Back,
        E_Up,
        E_Away,
        E_Left,
        E_Right,
        W_Straight
    }

    public BounceType bType;

    [SerializeField] float bounceForceMod;
    [HideInInspector] [Range(0.01f, 1f)] public float BounceAwayAngleThreshold;

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

        vectors[0] = new Vector3(dir.x, dir.y + .3f, dir.z) * bounceForceMod;
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
        Vector3 dir = (position - origin).normalized;

        //vectors[0] = position;
        /*if ((dir.y < -BounceAwayAngleThreshold && collision.contacts[0].normal.normalized.y > 0) || (dir.y > BounceAwayAngleThreshold && collision.contacts[0].normal.normalized.y < 0))
        {
            Debug.Log("Top/Bottom bounce");
            vectors[0] = new Vector3((2 * collision.transform.position.x) - position.x, position.y, (2 * collision.transform.position.z) - position.z);
            dir.y = -dir.y - .5f;
        } else { */

        //(2 * enemyTransform.position) - transform.position

        vectors[0] = new Vector3(collision.position.x + ((collision.localScale.x / 2) * dir.x), collision.position.y + (collision.localScale.y / 2), collision.position.z + ((collision.localScale.z / 2) * dir.z));
        dir.y = .2f;

        Debug.Log(vectors[0]);

        vectors[1] = transform.position;
        vectors[2] = new Vector3(dir.x, dir.y + .25f, dir.z) * bounceForceMod * 10;

        return vectors;
    }

    public Vector3[] PlayerBounceForward(Transform collision, Vector3 position, Vector3 dir)
    {
        Vector3[] vectors = new Vector3[2];

        vectors[0] = new Vector3(collision.position.x + ((collision.localScale.x / 2) * dir.x), collision.position.y + (collision.localScale.y / 2), collision.position.z + ((collision.localScale.z / 2) * dir.z));
        dir.y = .2f;

        Debug.Log(vectors[0]);

        vectors[1] = new Vector3(dir.x, dir.y + .25f, dir.z) * bounceForceMod;

        return vectors;
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(Bouncing))]
public class Bouncing_Editor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector(); // for other non-HideInInspector fields

        Bouncing script = (Bouncing)target;

        switch (script.bType)
        {
            case Bouncing.BounceType.E_Away:
                script.BounceAwayAngleThreshold = EditorGUILayout.FloatField("Bounce Away Angle Threshold", script.BounceAwayAngleThreshold);
                break;
        }
        /*script.iField = EditorGUILayout.ObjectField("I Field", script.iField, typeof(InputField), true) as InputField;
            script.Template = EditorGUILayout.ObjectField("Template", script.Template, typeof(GameObject), true) as GameObject;*/
    }
}
#endif