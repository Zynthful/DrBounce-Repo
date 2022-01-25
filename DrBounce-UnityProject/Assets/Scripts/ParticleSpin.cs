using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSpin : MonoBehaviour
{
    public Vector3 spinSpeed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(spinSpeed * Time.deltaTime);
    }
}
