using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class PlatformHeight : MonoBehaviour
{
    [SerializeField]
    private Transform[] objects = new Transform[0];

    [SerializeField]
    private float height = 1f;

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
            Destroy(this);
        }
    }

    private void Update()
    {
        float baseAndTopHeight = topSizeY + baseSizeY;
        float baseToTopHeight = height - baseAndTopHeight;
        float pieceHeight = baseToTopHeight / 6f;

        for (int i = 0; i < 6; i++)
        {
            objects[i].localPosition = pieceHeight * i * Vector3.up + (baseSizeY * Vector3.up);
        }

        objects[6].localPosition = objects[5].localPosition + ((pieceSizeY + baseSizeY - topSizeY) * Vector3.up);
        //objects[6].localPosition = objects[5].localPosition + ((pieceSizeY * Vector3.up) + (baseSizeY * Vector3.up) - (topSizeY * Vector3.up));
    }
}
