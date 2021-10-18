using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class BezierCurve3PointLineRenderer : MonoBehaviour
{
    private LineRenderer lineRenderer;

    [SerializeField] private Transform point1;
    [SerializeField] private Transform point2;
    [SerializeField] private Transform point3;

    [SerializeField] private int vertexCount = 12;

    private float colourDistance = 3.5f;

    [SerializeField] private Color colourClose = Color.blue;
    [SerializeField] private Color colourFar = Color.red;

    [SerializeField] private MagnetAA mAA = null;

    private bool magnetise = false;

    private void Start()
    {
        if (mAA != null)
        {
            colourDistance = mAA.aimAssistMaxRange;
        }

        lineRenderer = GetComponent<LineRenderer>();

        Gradient gradient = new Gradient();
        gradient.SetKeys(new GradientColorKey[] { new GradientColorKey(colourClose, 0.0f), new GradientColorKey(colourFar, 1.0f) }, new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 0.0f), new GradientAlphaKey(1.0f, 1.0f) });
        lineRenderer.colorGradient = gradient;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.s_Instance.paused) return;

        lineRenderer.enabled = magnetise;

        if (magnetise)
        {
            if (Vector3.Distance(point1.position, point3.position) > colourDistance || Vector3.Distance(point1.position, point3.position) < 1f)
            {
                lineRenderer.enabled = false;
                return;
            }
        }
        else 
            return;

        point2.localPosition = new Vector3(0, 0, Vector3.Distance(point1.position, point3.position) / 2);

        List<Vector3> pointList = new List<Vector3>();
        for (float ratio = 0; ratio <= 1; ratio += 1.0f / vertexCount)
        {
            Vector3 bezierpoint = QuadraticBezierCurve(ratio, point1.position, point2.position, point3.position);
            pointList.Add(bezierpoint);
        }

        lineRenderer.positionCount = pointList.Count;
        lineRenderer.SetPositions(pointList.ToArray());

        Gradient gradient = new Gradient();
        Color endColour = Color.Lerp(colourClose, colourFar, Mathf.Clamp(Vector3.Distance(point1.position, point3.position) / colourDistance, 0, 1));
        gradient.SetKeys(new GradientColorKey[] { new GradientColorKey(colourClose, 0.0f), new GradientColorKey(endColour, 1.0f / (Vector3.Distance(point1.position, point3.position) / colourDistance)) }, new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 0.0f), new GradientAlphaKey(1.0f, 1.0f) });
        lineRenderer.colorGradient = gradient;
    }

    public void isMag(bool mag)
    {
        Debug.Log("yes itsaworkin and its " + mag);
        magnetise = mag;
    }

    public Vector3 QuadraticBezierCurve(float t, Vector3 p0, Vector3 p1, Vector3 p2)
    {
        return (1.0f - t) * (1.0f - t) * p0 + 2.0f * (1.0f - t) * t * p1 + t * t * p2;
    }
}