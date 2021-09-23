using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSway : MonoBehaviour
{
    [SerializeField]
    private float bobSpeed = 1f;
    [SerializeField]
    private float bobDistance = 1f;
    [SerializeField]
    private float viewmodelSwayAmount = 1f;
    [SerializeField]
    private float viewmodelReturnToStartSpeed = 3f;
    [SerializeField]
    private float snappiness = 1f;

    private float timer = 0f;
    private float waveSlice = 0f;
    private float xVelocity = 0f;

    private Vector3 midPoint = new Vector3();

    void Start()
    {
        midPoint = transform.localPosition;
    }

    void Update()
    {
        if (transform.childCount != 0)
        {
            HorizontalSway();
            VerticalSway();
        }
    }

    private void HorizontalSway()
    {
        float moveX = -Input.GetAxis("Mouse X") * viewmodelSwayAmount;

        xVelocity = Mathf.Lerp(xVelocity, moveX, snappiness * Time.deltaTime);

        Vector3 targetDirection = Vector3.zero;

        float singleStep = viewmodelReturnToStartSpeed * Time.deltaTime;

        Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, singleStep, 0.0f);

        transform.localEulerAngles = newDirection;

        transform.Rotate(Vector3.up, xVelocity);
    }

    private void VerticalSway()
    {
        //change this to new input :)
        float vertical = Input.GetAxis("Vertical");
        float horizontal = Input.GetAxis("Horizontal");

        Vector3 localPosition = transform.localPosition;

        if (Mathf.Abs(horizontal) == 0 && Mathf.Abs(vertical) == 0)
            timer = 0.0f;
        else
        {
            waveSlice = Mathf.Sin(timer);
            timer = timer + bobSpeed;

            if (timer > Mathf.PI * 2)
                timer = timer - (Mathf.PI * 2);
        }

        if (waveSlice != 0)
        {
            //when the values change between 0 and 1 it smoothly interpolates which is a problem :(
            //maybe it can be fixed with the new input system

            float translateChange = waveSlice * bobDistance;
            float totalAxes = Mathf.Abs(horizontal) + Mathf.Abs(vertical);
            totalAxes = Mathf.Clamp(totalAxes, 0.0f, 1.0f);
            translateChange = totalAxes * translateChange;
            localPosition.z = midPoint.z + translateChange;
        }
        else
        {
            localPosition.z = midPoint.z;
        }

        transform.localPosition = localPosition;
    }
}
