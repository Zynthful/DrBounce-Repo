using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spin : MonoBehaviour
{
    private bool rotate;

    [SerializeField] private int rotationAmount = 1000;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (rotate) transform.rotation *= Quaternion.AngleAxis(rotationAmount * Time.deltaTime, Vector3.right);
    }

    private void OnCollisionEnter(Collision collision)
    {
        rotate = true;
        Invoke("OnStop", 4);
    }

    private void OnStop()
    {
        rotate = false;
    }
}
