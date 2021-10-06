using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class BezierCurve3PointLineRenderer : MonoBehaviour
{

    [SerializeField]
    private Transform point1;
    [SerializeField]
    private Transform point2;
    [SerializeField]
    private Transform point3;
    [SerializeField]
    private LineRenderer lineRenderer;

    [SerializeField]
    private int vertexCount = 12;

    // Update is called once per frame
    void Update()
    {
        if (!lineRenderer.enabled) { return; }

        point2.localPosition = new Vector3(0, 0, Vector3.Distance(point1.position, point3.position) / 2);

        List<Vector3> pointList = new List<Vector3>();
        for (float ratio = 0; ratio <= 1; ratio += 1.0f / vertexCount)
        {
            Vector3 bezierpoint = QuadraticBezierCurve(ratio, point1.position, point2.position, point3.position);
            pointList.Add(bezierpoint);
        }
        lineRenderer.positionCount = pointList.Count;
        lineRenderer.SetPositions(pointList.ToArray());
    }

    public Vector3 QuadraticBezierCurve(float t, Vector3 p0, Vector3 p1, Vector3 p2)
    {
        return (1.0f - t) * (1.0f - t) * p0 + 2.0f * (1.0f - t) * t * p1 + t * t * p2;
    }
}