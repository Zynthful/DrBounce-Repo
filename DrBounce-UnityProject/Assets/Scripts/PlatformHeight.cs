using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
public class PlatformHeight : MonoBehaviour
{
    [SerializeField]
    private Transform[] objects = new Transform[0];

    private float height;

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
        height = 4f;

        float baseAndTopHeight = topSizeY + baseSizeY;
        float baseToTopHeight = height - baseAndTopHeight;
        float pieceHeight = baseToTopHeight / 6f;

        for (int i = 0; i < 6; i++)
        {
            objects[i].localPosition = Mathf.Clamp((pieceHeight * i) + baseSizeY, baseSizeY, Mathf.Infinity) * Vector3.up;
        }

        objects[6].localPosition = Vector3.zero; //objects[5].localPosition + ((pieceSizeY + topSizeY) * Vector3.up);
    }
}
