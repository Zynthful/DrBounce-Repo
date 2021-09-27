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
    [SerializeField] private float rotateToStartSpeed = 3f;
    [SerializeField] private float snappiness = 1f;

    [Header("Dash/Jump Offset")]
    [SerializeField] private float returnToStartSpeed = 1f;
    [SerializeField] private float jumpDistance = 1f;
    [SerializeField] private float dashDistance = 1f;

    [Header("Falling")]
    [SerializeField] private float offsetMultiplier = 5f;
    [SerializeField] private float terminalVelocity = 15f;

    private float timer = 0f;
    private float waveSlice = 0f;
    private float xVelocity = 0f;

    private float verticalOld = 0f;

    public InputMaster controls;

    private Vector3 midPoint = new Vector3();
    
    [SerializeField] private PlayerMovement movement;

    private void Awake()
    {
        controls = new InputMaster();
        //controls.Player.Jump.performed += _ => Offset(Vector3.down, jumpDistance);
        //controls.Player.Dash.performed += _ => Offset(Vector3.back, dashDistance);
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
            //ReturnToStart();
            //Falling(movement.velocity.y);
        }
    }

    //brain rot

    private void HorizontalSway()
    {
        float moveX = -Mouse.current.delta.x.ReadValue() * swayAmount;

        xVelocity = Mathf.Lerp(xVelocity, moveX, snappiness * Time.deltaTime);

        transform.localEulerAngles = Vector3.RotateTowards(transform.forward, Vector3.zero, Time.deltaTime * rotateToStartSpeed, 0);

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
                timer -= Mathf.PI * 2;
        }

        if (waveSlice != 0)
        {
            float translateChange = waveSlice * bobDistance;
            float totalAxes = Mathf.Abs(input.y) + Mathf.Abs(input.x);
            totalAxes = Mathf.Clamp(totalAxes, 0, 1);

            verticalOld += ((totalAxes * 2) - 1) * bobTransitionSpeed; // when axes are big become big
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

    private void Falling(float yVelocity)
    {
        if (!movement.isGrounded && yVelocity != 0f)
        {
            yVelocity = 1 - (1 / (yVelocity + 1)); // make it between zero and one when there is no terminal velocity
            yVelocity = Mathf.Clamp(yVelocity, -1f, 1f);
            float offset = Mathf.Sqrt(Mathf.Abs((2f * yVelocity) - Mathf.Pow(yVelocity, 2f)));
            if (yVelocity > 0)
                offset *= -1;

            offset = Mathf.Clamp(offset, -1f, 1f);
            transform.localPosition = offset * offsetMultiplier * Vector3.up;
        }
        
    }

    private void Offset(Vector3 direction, float amount)
    {
        transform.localPosition += direction * amount;

        //naythumb
    }

    private void ReturnToStart()
    {
        //lerp toward midpoint
        transform.localPosition = Vector3.MoveTowards(transform.localPosition, midPoint, Time.deltaTime * returnToStartSpeed);
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
