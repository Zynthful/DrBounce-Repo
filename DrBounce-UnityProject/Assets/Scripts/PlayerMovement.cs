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

    [Header("Dashing")]
    public float dashStrength = 4f;
    public float dashLength = 0.2f;
    public int dashesBeforeLanding;
    public float cooldownTime = 0.5f;
    public bool noMoveAfterDashOnOff;
    public float noMovementTime;
    private bool cooldown = false;
    private bool isDashing = false;
    private int dashesPerformed = 0;
    private bool feedbackPlayed = false;

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

    [Header("Feedbacks")]
    public MMFeedbacks DashFeedback;

    [Header("Vibrations")]
    public VibrationManager vibrationManager;

    [Header("Events")]
    [SerializeField] private GameEvent onJump = null;
    [SerializeField] private GameEvent onDash = null;

    //Crouching:
    private bool isCrouching;
    private CharacterController charController;
    private float playerHeight;
    private float oldSpeed;
    //private float playerHeight;

    private void Awake()
    {
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
        float x = controls.Player.Movement.ReadValue<Vector2>().x; //Reads the value set from the Input Master based on which keys are being pressed, or where the player is holding on a joystick.
        float z = controls.Player.Movement.ReadValue<Vector2>().y;

        #region Crouching
        //print(isCrouching);
        float h = playerHeight;
        if (isCrouching == true) //If dash button is being held down, and the isCrouching is enabled by the dash coroutine
        {
            h = playerHeight * 0.5f;
        }
        float lastHeight = charController.height;
        charController.height = Mathf.Lerp(charController.height, h, 5 * Time.deltaTime);
        transform.localPosition += new Vector3(0, (charController.height - lastHeight) / 2, 0);

        #endregion
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, ~groundMask); //Returns true to isGrounded if a small sphere collider below the player overlaps with something with the ground Layer
        headIsTouchingSomething = Physics.CheckSphere(headCheck.position, headDistance, ~headMask);

        #region DashStopping
        if (isGrounded)
        {
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
        if (!GameManager.s_Instance.paused)
        {
            Vector3 move = (transform.right * x + transform.forward * z).normalized; //Creates a value to move the player in local space based on this value.
            controller.Move(move * speed * Time.deltaTime); //uses move value to move the player.
            velocity.y += gravity * Time.deltaTime; //Raises velocity the longer the player falls for.
            controller.Move(velocity * Time.deltaTime); //Moves the player based on this velocity.
        }
        #endregion

        #region Dashing
        if (isDashing == true)
        {
            if(feedbackPlayed == false)
            {
                DashFeedback?.PlayFeedbacks(); //Play feedback
                feedbackPlayed = true;
            }
            cooldown = true;

            if (controls.Player.Movement.ReadValue<Vector2>().y != 0) //If player is moving in the Y axis
            {
                Vector3 move2 = transform.forward * z;
                controller.Move(move2 * dashStrength * speed * Time.deltaTime); //Move them forward at a speed based on the dash strength
            }

            else if (controls.Player.Movement.ReadValue<Vector2>().x != 0) //Else if the player is moving in the X axis
            {
                Vector3 move2 = transform.right * x;
                controller.Move(move2 * dashStrength * speed * Time.deltaTime); //Do the same, to the side.
                //The X axis is an else if, as it ensures that if the player is holding both Up and Right on the arrowkeys while dashing, they only dash forward
            }
            else
            {
                Vector3 move2 = transform.forward;
                controller.Move(move2 * dashStrength * speed * Time.deltaTime); //Move them forward at a speed based on the dash strength
            }
        }
        #endregion

        #region jump

        if (jump == true)
        {
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
        if (!GameManager.s_Instance.paused && isGrounded)
        {
            if (prevJump == false)
            {
                prevJump = true;
                jumpHeight += (jumpMin);
            }

            gravity = prevGrav;
            if(isCrouching == true)
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


            StartCoroutine(NoMoveAfterDash());
            
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
        if(dashesPerformed < dashesBeforeLanding)
        {
            isDashing = true; //Set isDashing to true, which allows the if(dashing is true) statement in Update to start

            vibrationManager.DashVibration();
            onDash?.Raise();

            velocity.y = 0;
            jumpHeight = 0;
            jump = false;

            float oldGravity = gravity;
            gravity = 0;

            yield return new WaitForSeconds(dashLength); //Continue this if statement every frame for the set dash length

            gravity = oldGravity;
            dashesPerformed += 1;

            isDashing = false;
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

    IEnumerator NoMoveAfterDash()
    {
        yield return new WaitForSeconds(dashLength);
        if(noMoveAfterDashOnOff == true)
        {
            float previousSpeed = speed;
            speed = 0;
            yield return new WaitForSeconds(noMovementTime);
            speed = previousSpeed;
        }
    }

    private void OnEnable() //Enables and disables the local version of controls as the gameobject is enabled and disabled.
    {
        controls.Enable();
    }

    private void OnDisable()//Brackeys Moment
    {
        controls.Disable();
    }
}