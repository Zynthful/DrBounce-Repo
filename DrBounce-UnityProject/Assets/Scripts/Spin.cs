using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spin : MonoBehaviour
{
    private bool rotate;
    private float newRotate = 0f;

    private float spinTimer = 0f;

    private float hitDirection = 0f;

    [SerializeField] private int rotationAmount = 1000;
    [SerializeField] private float spinLength = 4f;

    //bullets interact

    // Update is called once per frame
    void Update()
    {
        if (rotate)
        {
            if (spinTimer <= 0f)
            {
                OnStop();
            }
            else
            {
                float t = spinTimer / spinLength;
                newRotate = Mathf.Lerp(rotationAmount, 0, Mathf.SmoothStep(1f, 0f, t));

                float x = newRotate * Time.deltaTime;
                transform.rotation *= Quaternion.AngleAxis(x, Vector3.right * hitDirection);

                spinTimer -= Time.deltaTime;
            }
        }

        //rotate towards ground
        if (transform.rotation.x != 0)
        {
            transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(0f, 0f, 0f), Time.deltaTime);
        }
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        hitDirection = Vector3.Dot(collision.transform.forward, transform.position);
        OnStart(hitDirection);
    }
    
    private void OnStop()
    {
        spinTimer = spinLength;
        rotate = false;
    }

    public void OnStart(float value) 
    {
        hitDirection = value;

        //good enough :)
        spinTimer = spinLength > 0 ? 1f : -1f;

        spinTimer = spinLength;
        rotate = true;
    }
}
