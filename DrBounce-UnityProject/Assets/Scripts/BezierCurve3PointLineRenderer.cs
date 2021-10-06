using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(LineRenderer))]
public class BezierCurve3PointLineRenderer : MonoBehaviour
{

    [SerializeField]
    private Transform point1;
    [SerializeField]
    private Transform point2;
    [SerializeField]
    private Transform point3;

    private LineRenderer lineRenderer;

    [SerializeField]
    private int vertexCount = 12;

    [SerializeField]
    private float colourDistance = 20f;

    private Material mat = null;

    [SerializeField]
    private Color colourClose = Color.blue;

    [SerializeField]
    private Color colourFar = Color.red;


    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        mat = lineRenderer.material;

        lineRenderer.startColor = colourClose;
        lineRenderer.endColor = colourFar;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.s_Instance.paused) { return; }

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

        mat.SetColor("_Color", Color.Lerp(colourClose, colourFar, Mathf.Clamp(Vector3.Distance(point1.position, point3.position) / colourDistance, 0, 1)));
    } 

    public Vector3 QuadraticBezierCurve(float t, Vector3 p0, Vector3 p1, Vector3 p2)
    {
        return (1.0f - t) * (1.0f - t) * p0 + 2.0f * (1.0f - t) * t * p1 + t * t * p2;
    }
}