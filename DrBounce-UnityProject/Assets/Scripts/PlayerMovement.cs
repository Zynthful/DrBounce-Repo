using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using MoreMountains.Feedbacks;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;

    [Header("Base Movement")]
    public float speed = 8f;
    public float gravity = -19.81f;
    public static Transform player;
    public InputMaster controls;

    [Header("Jump")]
    public float jumpPeak = 3f;
    public float jumpMin = 1f;
    public float floatiness;
    private bool jump = false;
    private float jumpHeight = 0f;
    private bool prevJump = false;
    private float prevGrav;
    private float mayJump;
    private bool hasJumped = false;

    [Header("Dashing")]
    public float dashStrength = 4f;
    public float dashLength = 0.2f;
    public int dashesBeforeLanding;
    public float cooldownTime = 0.5f;
    public float extendedNoGravTime = 0.1f;
    public float noMovementTime;
    private bool cooldown = false;
    private bool isDashing = false;
    private int dashesPerformed = 0;
    private bool feedbackPlayed = false;
    private bool dashLocker = false;
    private bool movementBlocker = false;
    private bool hasDashed = false;
    private float x2;
    private float z2;

    [Header("Ground+Head Checking")]
    public Transform groundCheck;
    public Transform headCheck;
    public LayerMask groundMask;
    public LayerMask headMask;
    public float groundDistance = 0.4f;
    public float headDistance = 0.4f;
    [HideInInspector] public bool isGrounded;
    [HideInInspector] public bool headIsTouchingSomething;
    public Vector3 velocity;
    private float oldGroundDistance;
    private bool hasIncreasedGroundDistance = false;

    [Header("Feedbacks")]
    public MMFeedbacks DashFeedback;

    [Header("Vibrations")]
    public VibrationManager vibrationManager;

    [Header("Events")]
    [SerializeField] private GameEvent onJump = null;
    [SerializeField] private GameEvent onDash = null;

    //Crouching:
    public bool isCrouching;
    private CharacterController charController;
    private float playerHeight;
    private float oldSpeed;
    //private float playerHeight;

    private void Awake()
    {
        oldGroundDistance = groundDistance;
        prevGrav = gravity;
        charController = GetComponent<CharacterController>();
        playerHeight = charController.height;

        controls = new InputMaster(); //Creates a new InputMaster to gain access to mapped controls
        controls.Player.Jump.performed += _ => Jump(); //When the jump action is activated in Input Master, activate the Jump function.
        controls.Player.Dash.performed += _ => Dash();
        controls.Player.Crouch.performed += _ => Crouch();
        player = transform;
    }
    void FixedUpdate()
    {

        #region Crouching
        //print(isCrouching);
        float h = playerHeight;
        if (isCrouching == true) //If dash button is being held down, and the isCrouching is enabled by the dash coroutine
        {
            h = playerHeight * 0.35f;
        }
        float lastHeight = charController.height;
        charController.height = Mathf.Lerp(charController.height, h, 5 * Time.deltaTime);
        transform.localPosition += new Vector3(0, (charController.height - lastHeight) / 2, 0);

        #endregion

        #region GroundChecking
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, ~groundMask); //Returns true to isGrounded if a small sphere collider below the player overlaps with something with the ground Layer
        headIsTouchingSomething = Physics.CheckSphere(headCheck.position, headDistance, ~headMask);

        mayJump -= Time.deltaTime;
        if (isGrounded)
        {
            mayJump = 0.5f;
            hasJumped = false;
            dashesPerformed = 0;
            if (dashesPerformed > 0)
            {
                StartCoroutine(StopDash()); //Starts coroutine stopdash, which waits a split second after hitting the ground to reset the dash counter.
            } //This delay is to prevent the player being able to dash just before they hit the ground.
            if (velocity.y < 0) //If player is grounded and velocity is lower than 0, set it to 0.
            {
                velocity.y = (-40f * Time.fixedDeltaTime);
            }
        }

        if (headIsTouchingSomething)
        {
            velocity.y = (-40f * Time.fixedDeltaTime);
            if (isCrouching == true)
            {
                isGrounded = false;
                cooldown = true;
            }
        }

        if(!headIsTouchingSomething && isCrouching == true)
        {
            isGrounded = true;
            cooldown = false;
        }
        #endregion

        #region Movement
        if (!GameManager.s_Instance.paused && movementBlocker == false)
        {

            float x = controls.Player.Movement.ReadValue<Vector2>().x; //Reads the value set from the Input Master based on which keys are being pressed, or where the player is holding on a joystick.
            float z = controls.Player.Movement.ReadValue<Vector2>().y;

            Vector3 move = (transform.right * x + transform.forward * z).normalized; //Creates a value to move the player in local space based on this value.
            controller.Move(move * speed * Time.deltaTime); //uses move value to move the player.
        }

        velocity.y += gravity * Time.deltaTime; //Raises velocity the longer the player falls for.
        controller.Move(velocity * Time.deltaTime); //Moves the player based on this velocity.

        #endregion

        #region Dashing
        if (isDashing == true)
        {
            if(hasDashed == false)
            {
                if (feedbackPlayed == false)
                {
                    DashFeedback?.PlayFeedbacks(); //Play feedback
                    feedbackPlayed = true;
                }
                cooldown = true;

                if (dashLocker == false)
                {
                    dashLocker = true;
                    x2 = controls.Player.Movement.ReadValue<Vector2>().x;
                    z2 = controls.Player.Movement.ReadValue<Vector2>().y;
                }
                Vector3 move = (transform.right * x2 + transform.forward * z2).normalized;

                controller.Move(move * dashStrength * speed * Time.deltaTime);
            }


            if (controls.Player.Movement.ReadValue<Vector2>().x == 0 && controls.Player.Movement.ReadValue<Vector2>().y == 0)
            {
                Vector3 move2 = transform.forward;
                controller.Move(move2 * dashStrength * speed * Time.deltaTime); //Move them forward at a speed based on the dash strength
                hasDashed = true;
                controls.Player.Movement.Disable();
            }
        }
        #endregion

        #region jump

        if (jump == true)
        {
            hasJumped = true;

            velocity.y = (Mathf.Sqrt(jumpHeight * -2 * gravity));

            if (controls.Player.Jump.ReadValue<float>() == 1)
            {

                jumpHeight += (5f * Time.fixedDeltaTime);
            }

            else
            {
                jump = false;
                print("Midhop");
                jumpHeight = 0;
                velocity.y -= floatiness;

            }

            if (jumpHeight >= jumpPeak)
            {
                jump = false;
                jumpHeight = 0;
                velocity.y -= floatiness;

            }
        }

        #endregion
    }
    private void Jump()
    {
        if (!GameManager.s_Instance.paused && mayJump > 0 && hasJumped == false)
        {
            
            if (prevJump == false)
            {
                prevJump = true;
                jumpHeight += (jumpMin);
            }

            gravity = prevGrav;
            if (isCrouching == true)
            {
                Crouch(); //Un-crouches the player before jumping
            }

            gravity = prevGrav;
            isDashing = false;
            feedbackPlayed = false;
            prevJump = false;

            jump = true;
            vibrationManager.JumpVibration();
            onJump?.Raise();
        }
        else
        {
            print("Test");
            
        }
    }

    void Dash()
    {
        // Checks:
        // - If the game isn't paused
        // - Not on the ground
        // - Not cooling down
        // - Not already dashing
        if (!GameManager.s_Instance.paused && isGrounded != true && cooldown == false && isDashing == false)
        {
            StartCoroutine(EnableDisableDash());

            isCrouching = false;
            StartCoroutine(CoolDownTest());
        }
    }

    void Crouch()
    {
        if(!GameManager.s_Instance.paused && isGrounded == true)
        {
            if (isCrouching == true)
            {
                isCrouching = false;
                speed = oldSpeed;
            }
            else
            {
                isCrouching = true;
                oldSpeed = speed;
                speed /= 2;
            }
        }
    }

    IEnumerator EnableDisableDash()
    {
        if (dashesPerformed < dashesBeforeLanding)
        {
            isDashing = true; //Set isDashing to true, which allows the if(dashing is true) statement in Update to start
            movementBlocker = true;

            vibrationManager.DashVibration();
            onDash?.Raise();

            velocity.y = 0;
            jumpHeight = 0;
            jump = false;

            float oldGravity = gravity;
            gravity = 0;

            yield return new WaitForSeconds(dashLength); //Continue this if statement every frame for the set dash length

            dashesPerformed += 1;

            isDashing = false;
            dashLocker = false;
            movementBlocker = false;
            hasDashed = false;
            controls.Player.Movement.Enable();

            yield return new WaitForSeconds(extendedNoGravTime);
            gravity = oldGravity;
        }
    }

    IEnumerator CoolDownTest()
    {
        yield return new WaitForSeconds(cooldownTime); //If the cooldown is active, wait for cooldown time set, until setting cooldown as false
        cooldown = false;
        if(dashesPerformed < dashesBeforeLanding)
        {
            feedbackPlayed = false;
        }
    }
    IEnumerator StopDash()
    {
        yield return new WaitForSeconds(0.1f);
        dashesPerformed = 0;
    }

    #region BrackeysMoment
    private void OnEnable() //Enables and disables the local version of controls as the gameobject is enabled and disabled.
    {
        controls.Enable();
    }

    private void OnDisable()//Brackeys Moment
    {
        controls.Disable();
    }
    #endregion
}