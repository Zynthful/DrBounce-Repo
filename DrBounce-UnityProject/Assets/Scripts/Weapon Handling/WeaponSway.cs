using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponSway : MonoBehaviour
{
    [Header("Vertical Bob")]
    [SerializeField] private float bobSpeed = 1f; // this should be added to velocity in future
    [SerializeField] private float bobDistance = 1f; // max distance of bob
    [SerializeField] private float bobTransitionSpeed = 0.01f;

    [Header("Vertical Bob Crouched")]
    [SerializeField] private float bobSpeedC = .5f; // this should be added to velocity in future
    [SerializeField] private float bobDistanceC = .5f; // max distance of bob
    [SerializeField] private float bobTransitionSpeedC = 0.005f;

    [Header("Horizontal Sway")]
    [SerializeField] private float swayAmount = 1f;
    [SerializeField] private float rotateToStartSpeed = 3f;
    [SerializeField] private float snappiness = 1f;

    [Header("Dash/Jump Offset")]
    [SerializeField] private float returnToBobSpeed = 1f;
    [SerializeField] private float jumpDistance = 1f;
    [SerializeField] private float dashDistance = 1f;

    [Header("Falling")]
    [SerializeField] private float offsetMultiplier = 5f;
    [SerializeField] private float offsetMax = 1f;

    [Header("Sliding")]
    [SerializeField] private float slideOffset = -0.25f;

    private float timer = 0f;
    private float waveSlice = 0f;
    private float xVelocity = 0f;

    private float verticalOld = 0f;
    private float speedOld = 0f;

    private InputMaster controls;

    private Vector3 midPoint = new Vector3();
    
    [SerializeField] private PlayerMovement movement;

    public static Transform weaponHolderTransform;

    private void Awake()
    {
        controls = InputManager.inputMaster;

        weaponHolderTransform = transform;
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }

    private void Start()
    {
        midPoint = transform.localPosition;
    }

    private void Update()
    {
        if (transform.childCount != 0)
        {
            Vector3 bobPos, newPos;

            if (!movement.GetIsCrouching())
            {
                bobPos = CalculateVerticalBob(bobSpeed, bobDistance, bobTransitionSpeed);
            }
            else
            {
                bobPos = CalculateVerticalBob(bobSpeedC, bobDistanceC,bobTransitionSpeedC);
            }

            HorizontalSway();

            if (movement.isSliding)
            {
                bobPos = Vector3.up * slideOffset;
            }
            
            newPos = MoveTowardsBob(transform.localPosition, bobPos);
            newPos = Falling(movement.velocity.y, newPos);

            transform.localPosition = newPos;
        }
    }

    // why is this the easiest part of the feature
    private void HorizontalSway()
    {
        float moveX = -Mouse.current.delta.x.ReadValue() * swayAmount;

        if (Gamepad.current != null)
        {
            moveX = -Gamepad.current.rightStick.x.ReadValue() * swayAmount;
        }
        
        xVelocity = Mathf.Lerp(xVelocity, moveX, snappiness * Time.deltaTime);

        transform.localEulerAngles = Vector3.RotateTowards(transform.forward, Vector3.zero, Time.deltaTime * rotateToStartSpeed, 0);

        transform.Rotate(Vector3.up, xVelocity);

        transform.localEulerAngles = new Vector3(0, transform.localEulerAngles.y, 0);
    }

    // this will work a lot better if i can get the players velocity directly but for now scuffed method will do :)
    // i agree :) -jamie
    private Vector3 CalculateVerticalBob(float bobS, float bobD, float bobTS)
    {
        Vector2 input = controls.Player.Movement.ReadValue<Vector2>();
        Vector3 position = Vector3.up * verticalOld;

        if (Mathf.Abs(input.x) == 0 && Mathf.Abs(input.y) == 0)
        {
            if (speedOld == 0)
            {
                return position;
            }

            timer = 0;
        }
            
        else
        {
            waveSlice = (Mathf.Sin(timer) + 1) * 0.5f;
            timer += bobS;

            if (timer > Mathf.PI * 2)
                timer -= Mathf.PI * 2;
        }

        if (waveSlice != 0)
        {
            float translateChange = waveSlice * bobD;
            float totalAxes = Mathf.Abs(input.y) + Mathf.Abs(input.x);
            totalAxes = Mathf.Clamp(totalAxes, 0, 1);

            speedOld += ((totalAxes * 2) - 1) * bobTS; // when axes are big become big
            speedOld = Mathf.Clamp(speedOld, 0, 1);

            translateChange *= speedOld;
            position.y = midPoint.y - translateChange;
        }
        else
        {
            position.y = midPoint.y;
        }

        verticalOld = position.y;

        return position;
    }

    private Vector3 Falling(float yVelocity, Vector3 pos)
    {
        if (!movement.isGrounded)
        {
            float fallingOffset = Mathf.Clamp(Mathf.Pow(Mathf.Clamp(yVelocity, -10, 10), offsetMultiplier), offsetMax * -1, offsetMax) * Time.deltaTime;
            pos.y -= fallingOffset;
        }

        return pos;

        // this code is not what we need but i just dont want to lose it :(
        /*
        if (!movement.isGrounded && yVelocity != 0f)
        {
            yVelocity = yVelocity / terminalVelocity;
            yVelocity = Mathf.Clamp(yVelocity, -1f, 1f);
            float offset = Mathf.Sqrt(Mathf.Abs((2f * yVelocity) - Mathf.Pow(yVelocity, 2f)));
            if (yVelocity > 0)
                offset *= -1;

            offset = Mathf.Clamp(offset, -1f, 1f);
            transform.localPosition = offset * offsetMultiplier * Vector3.up;
        }
        */
    }

    private Vector3 MoveTowardsBob(Vector3 position, Vector3 bobPosition)
    {
        float speed = Mathf.Pow(Vector3.Distance(position, bobPosition), returnToBobSpeed);
        return Vector3.MoveTowards(position, bobPosition, Time.deltaTime * speed);
    }

    // check for if the action is avaliable in future
    private void Offset(Vector3 direction, float amount)
    {
        transform.localPosition += direction * amount;

        //naythumb
    }

    public void Jump()
    {
        Offset(Vector3.down, jumpDistance);
    }

    public void Dash()
    {
        Offset(Vector3.back, dashDistance);
    }
}
