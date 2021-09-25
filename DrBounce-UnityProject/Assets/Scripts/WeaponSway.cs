using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponSway : MonoBehaviour
{
    [Header("Forward Bob")]
    [SerializeField] private float bobSpeed = 1f; // this should be added to velocity in future
    [SerializeField] private float bobDistance = 1f; // max distance of bob
    [SerializeField] private float bobTransitionSpeed = 0.01f;

    [Header("Horizontal Sway")]
    [SerializeField] private float swayAmount = 1f;
    [SerializeField] private float returnToStartSpeed = 3f;
    [SerializeField] private float snappiness = 1f;

    private float timer = 0f;
    private float waveSlice = 0f;
    private float xVelocity = 0f;

    private float verticalOld = 0f;

    public InputMaster controls;

    private Vector3 midPoint = new Vector3();

    private void Awake()
    {
        controls = new InputMaster();
    }

    void Start()
    {
        midPoint = transform.localPosition;
    }

    void Update()
    {
        if (transform.childCount != 0)
        {
            HorizontalSway();
            VerticalBob();
        }
    }

    private void HorizontalSway()
    {
        float moveX = -Mouse.current.delta.x.ReadValue() * swayAmount;

        xVelocity = Mathf.Lerp(xVelocity, moveX, snappiness * Time.deltaTime);

        transform.localEulerAngles = Vector3.RotateTowards(transform.forward, Vector3.zero, Time.deltaTime * returnToStartSpeed, 0);

        transform.Rotate(Vector3.up, xVelocity);

        transform.localEulerAngles = new Vector3(0, transform.localEulerAngles.y, 0);
    }

    // this will work a lot better if i can get the players velocity directly but for now scuffed method will do :)
    private void VerticalBob()
    {
        Vector2 input = controls.Player.Movement.ReadValue<Vector2>();
        Vector3 localPosition = transform.localPosition;

        if (Mathf.Abs(input.x) == 0 && Mathf.Abs(input.y) == 0)
            timer = 0;
        else
        {
            waveSlice = (Mathf.Sin(timer) + 1) * 0.5f;
            timer += bobSpeed;

            if (timer > Mathf.PI * 2)
                timer -= (Mathf.PI * 2);
        }

        if (waveSlice != 0)
        {
            float translateChange = waveSlice * bobDistance;
            float totalAxes = Mathf.Abs(input.y) + Mathf.Abs(input.x);
            totalAxes = Mathf.Clamp(totalAxes, 0, 1);

            verticalOld += ((totalAxes * 2) - 1) * bobTransitionSpeed;
            verticalOld = Mathf.Clamp(verticalOld, 0, 1);

            translateChange *= verticalOld;
            localPosition.y = midPoint.y - translateChange;
        }
        else
        {
            localPosition.y = midPoint.y;
        }

        transform.localPosition = localPosition;
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }
}
