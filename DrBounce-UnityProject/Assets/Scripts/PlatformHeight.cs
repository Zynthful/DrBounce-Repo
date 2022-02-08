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

    private void Update()
    {
        float newHeight = height + 0.1f;
        float pieceHeight = newHeight / 6f;

        for (int i = 0; i < 6; i++)
        {
            objects[i].localPosition = pieceHeight * i * Vector3.up;
        }

        objects[6].localPosition = (0.75f + newHeight - (newHeight / 6f)) * Vector3.up;
    }
}
