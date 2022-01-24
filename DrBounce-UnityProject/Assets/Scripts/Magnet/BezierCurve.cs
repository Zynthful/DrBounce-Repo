using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BezierCurve : MonoBehaviour
{
    Vector2[] positions;

    public BezierCurve(Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3)
    {
        positions = new Vector2[4]
        {
            p0, p1, p2, p3
        };
    }
    public Vector2 ReturnCurve(float t)
    {
        return (Mathf.Pow((1 - t), 3) * positions[0]) + (3 * Mathf.Pow((1 - t), 2) * t * positions[1]) + (3 * (1 - t) * Mathf.Pow(t, 2) * positions[2]) + (Mathf.Pow(t, 3) * positions[3]);
    }
}