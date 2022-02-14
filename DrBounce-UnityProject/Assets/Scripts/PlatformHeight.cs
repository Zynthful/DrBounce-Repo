using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
public class PlatformHeight : MonoBehaviour
{
    [SerializeField]
    private Transform[] objects = new Transform[0];

    [SerializeField]
    private Transform heightPoint;

    [SerializeField]
    private float baseSizeY = 0.1f;

    [SerializeField]
    private float topSizeY = 0.1f;

    [SerializeField]
    private float pieceSizeY = 0.5f;

    private void Start()
    {
        if (Application.IsPlaying(gameObject))
        {
            Destroy(heightPoint.gameObject);
            Destroy(this);
        }
    }

    private void Update()
    {
        

        float height = heightPoint.localPosition.y;

        float baseAndTopHeight = topSizeY + baseSizeY;
        float baseToTopHeight = height - baseAndTopHeight;
        float pieceHeight = (1f / 6f) * baseToTopHeight;

        for (int i = 0; i < 7; i++)
        {
            objects[i].localPosition = Mathf.Clamp((pieceHeight * i) + baseSizeY + pieceHeight, baseSizeY, Mathf.Infinity) * Vector3.up;
        }

        objects[6].localPosition = ((pieceHeight * 6) + baseSizeY) * Vector3.up;

        if(gameObject.GetComponent<BoxCollider>() != null)
        {
            BoxCollider col = gameObject.GetComponent<BoxCollider>();
            col.size = new Vector3(col.size.x, height, col.size.z);
            col.center = new Vector3(col.center.x, height / 2, col.center.z);
        }
    }
}
