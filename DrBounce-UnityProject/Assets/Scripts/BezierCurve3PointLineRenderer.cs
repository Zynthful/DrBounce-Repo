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
    private float oldDistance = 0f;

    [Header("Events")]
    [SerializeField]
    private GameEventBool onIsActiveAndInRange = null;

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
        if (GameManager.s_Instance.paused) 
            return;

        float distance = Vector3.Distance(point1.position, point3.position);

        lineRenderer.enabled = magnetise;

        if (magnetise)
        {
            // Is it out of range or within 1m (i.e. being held)?
            if (distance > colourDistance || distance < 1f)
            {
                lineRenderer.enabled = false;
                onIsActiveAndInRange?.Raise(false);
                return;
            }
            else
            {
                onIsActiveAndInRange?.Raise(true);
            }
        }
        else 
            return;

        if (distance == oldDistance)
            return;

        point2.localPosition = new Vector3(0, 0, distance / 2);

        List<Vector3> pointList = new List<Vector3>();
        for (float ratio = 0; ratio <= 1; ratio += 1.0f / vertexCount)
        {
            Vector3 bezierpoint = QuadraticBezierCurve(ratio, point1.position, point2.position, point3.position);
            pointList.Add(bezierpoint);
        }

        lineRenderer.positionCount = pointList.Count;
        lineRenderer.SetPositions(pointList.ToArray());

        Gradient gradient = new Gradient();
        Color endColour = Color.Lerp(colourClose, colourFar, Mathf.Clamp(distance / colourDistance, 0, 1));
        gradient.SetKeys(new GradientColorKey[] { new GradientColorKey(colourClose, 0.0f), new GradientColorKey(endColour, 1.0f / (distance / colourDistance)) }, new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 0.0f), new GradientAlphaKey(1.0f, 1.0f) });
        lineRenderer.colorGradient = gradient;

        oldDistance = distance;
    }

    public void isMag(bool mag)
    {
        magnetise = mag;
    }

    public Vector3 QuadraticBezierCurve(float t, Vector3 p0, Vector3 p1, Vector3 p2)
    {
        return (1.0f - t) * (1.0f - t) * p0 + 2.0f * (1.0f - t) * t * p1 + t * t * p2;
    }
}