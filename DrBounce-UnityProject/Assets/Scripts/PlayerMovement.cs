using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;

    private bool isCrouching;
    private CharacterController charController;
    private float playerHeight;
    private float oldSpeed;

    [Header("Base Movement")]
    public float speed = 8f;
    public float gravity = -19.81f;
    public static Transform player;
    public InputMaster controls;
    public Vector3 move;
    [SerializeField]
    private float acceleration;
    public float accelerationSpeed;
    private bool isMoving = false;

    [Header("Jump")]
    public float jumpPeak = 3f;
    public float jumpMin = 1f;
    [Tooltip("The higher the value, the heavier the player is.")]
    public float floatiness;
    [Tooltip("Set between 1 and 0, with 1 being lots of time and 0 being none")]
    public float coyoteTime;
    private float oldCoyoteTime;
    private bool jump = false;
    private float jumpHeight = 0f;
    private bool prevJump = false;
    private float prevGrav;
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
    private bool dashLocker = false;
    private bool movementBlocker = false;
    private bool hasDashed = false;
    private float x2;
    private float z2;
    [SerializeField]
    private TextMeshProUGUI dashUI;
    [SerializeField]
    private Slider timeBeforeDash;

    private float dashSliderTime = 0f;

    [Header("Sliding")]
    public float slideTime;
    public float slideStrength;
    public bool isSliding = false;
    public float strafeStrength;
    public float slideGravity;
    private bool slideDirectionDecided = false;
    private Vector3 slideDirection;
    private Vector3 slideLeftRight;
    private bool hasLetGo = false;

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

    [Header("UnityEvents")]
    [SerializeField]
    private UnityEvent onJump = null;
    [SerializeField]
    private UnityEvent onDash = null;
    [SerializeField]
    private UnityEvent onSlide = null;
    [SerializeField]
    private UnityEvent onSlideEnd = null;

    [Header("Game Events")]
    [SerializeField]
    private GameEvent _onJump = null;
    [SerializeField]
    private GameEvent _onDash = null;
    [SerializeField]
    private GameEvent _onSlide = null;
    [SerializeField]
    private GameEvent _onSlideEnd = null;

    private void Awake()
    {
        oldCoyoteTime = coyoteTime;
        oldGroundDistance = groundDistance;
        prevGrav = gravity;
        charController = GetComponent<CharacterController>();
        playerHeight = charController.height;
        GameManager.gravity = gravity;

        controls = new InputMaster(); //Creates a new InputMaster to gain access to mapped controls
        controls.Player.Jump.performed += _ => Jump(); //When the jump action is activated in Input Master, activate the Jump function.
        controls.Player.Dash.performed += _ => StartCoroutine(Dash());
        controls.Player.Crouch.performed += _ => Crouch();
        player = transform;
    }

    private void FixedUpdate()
    {
        //@cole :)
        if (isDashing == true)
        {
            dashSliderTime += Time.deltaTime;

            float b = (1f - ((float)dashesPerformed / (float)dashesBeforeLanding)) * 100f;
            float a = (1f - (((float)dashesPerformed + 1f) / (float)dashesBeforeLanding)) * 100f;
            float t = 1f - Mathf.Clamp(dashSliderTime / dashLength, 0, 1);
            float dashSliderPos = Mathf.Lerp(a, b, Mathf.SmoothStep(0, 1, t));
            timeBeforeDash.value = dashSliderPos;
        }


        #region Crouching
        //print(isCrouching);
        float h = playerHeight;
        if (isCrouching == true) //If dash button is being held down, and the isCrouching is enabled by the dash coroutine
        {
            h = playerHeight * 0.35f;
        }
        if(isSliding == false)
        {
            float lastHeight = charController.height;
            charController.height = Mathf.Lerp(charController.height, h, 5 * Time.deltaTime);
            transform.localPosition += new Vector3(0, (charController.height - lastHeight) / 2, 0);
            groundCheck.transform.localPosition -= new Vector3(0, (charController.height - lastHeight) / 2, 0); //Moves the Grounch check inversely
        }

        #endregion

        #region GroundChecking

        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, ~groundMask); //Returns true to isGrounded if a small sphere collider below the player overlaps with something with the ground Layer
        headIsTouchingSomething = Physics.CheckSphere(headCheck.position, headDistance, ~headMask);

        coyoteTime -= Time.deltaTime;
        if (isGrounded)
        {
            coyoteTime = oldCoyoteTime;
            hasJumped = false;
            dashesPerformed = 0;

            dashUI.text = ("Dashes: " + (dashesBeforeLanding - dashesPerformed));
            timeBeforeDash.value = 100;

            if (dashesPerformed > 0)
            {
                StartCoroutine(StopDash()); //Starts coroutine stopdash, which waits a split second after hitting the ground to reset the dash counter.
            } //This delay is to prevent the player being able to dash just before they hit the ground.
            if (velocity.y < 0) //If player is grounded and velocity is lower than 0, set it to 0.
            {
                velocity.y = (-40f * Time.fixedDeltaTime);
                velocity.x = 0;
                velocity.z = 0;
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

        if (!headIsTouchingSomething && isCrouching == true)
        {
            isGrounded = true;
            cooldown = false;
        }
        #endregion

        #region Movement
        if (!GameManager.s_Instance.paused && movementBlocker == false)
        {

            acceleration += Time.deltaTime * accelerationSpeed;

            if (acceleration >= 1)
            {
                acceleration = 1;
            }

            float x = controls.Player.Movement.ReadValue<Vector2>().x; //Reads the value set from the Input Master based on which keys are being pressed, or where the player is holding on a joystick.
            float z = controls.Player.Movement.ReadValue<Vector2>().y;

            if (isSliding == false)
            {
                move = (transform.right * x + transform.forward * z).normalized * acceleration; //Creates a value to move the player in local space based on this value.
                controller.Move(move * speed * Time.deltaTime); //uses move value to move the player.
            }
            else
            {
                move = (slideLeftRight * x); //Creates a value to move the player in local space based on this value.
                controller.Move(move * strafeStrength * Time.deltaTime); //uses move value to move the player.
            }

            // Check if moving
            isMoving = move != Vector3.zero ? true : false;
        }

        if (controls.Player.Movement.ReadValue<Vector2>().x == 0 && controls.Player.Movement.ReadValue<Vector2>().y == 0)
        {
            acceleration = 0;
        }
        //EARLY MOMENTUM SYSTEM - DOESN'T RESET! 
        //velocity.x += move.x;
        //velocity.z += move.z;

        velocity.y += gravity * Time.deltaTime; //Raises velocity the longer the player falls for.
        controller.Move(velocity * Time.deltaTime); //Moves the player based on this velocity.

        #endregion

        #region Dashing
        if (isDashing == true)
        {
            velocity = Vector3.zero;

            if(hasDashed == false)
            {
                //acceleration = 1;
                cooldown = true;

                if (dashLocker == false)
                {
                    dashLocker = true;
                    x2 = controls.Player.Movement.ReadValue<Vector2>().x;
                    z2 = controls.Player.Movement.ReadValue<Vector2>().y;
                }
                move = (transform.right * x2 + transform.forward * z2).normalized;

                controller.Move(move * dashStrength * speed * Time.deltaTime);
            }

            if (controls.Player.Movement.ReadValue<Vector2>().x == 0 && controls.Player.Movement.ReadValue<Vector2>().y == 0)
            {
                move = transform.forward;
                controller.Move(move * dashStrength * speed * Time.deltaTime); //Move them forward at a speed based on the dash strength
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

        #region Slide
        if (isSliding == true)
        {
            if(controls.Player.Crouch.ReadValue<float>() == 0)
            {
                hasLetGo = true;
            }
            if (controls.Player.Crouch.ReadValue<float>() == 1 && hasLetGo == true)
            {
                isSliding = false;
            }
            acceleration = 1;
            coyoteTime = oldCoyoteTime;
            gravity = slideGravity;
            print(isGrounded);
            isGrounded = true;
            if (slideDirectionDecided == false)
            {
                slideDirectionDecided = true;
                slideDirection = transform.forward;
                slideLeftRight = transform.right;
            }

            cooldown = true;
            h = playerHeight * 0.35f;
            float lastHeight = charController.height;
            charController.height = Mathf.Lerp(charController.height, h, 20 * Time.deltaTime);
            transform.localPosition += new Vector3(0, (charController.height - lastHeight) / 2, 0);
            groundCheck.transform.localPosition -= new Vector3(0, (charController.height - lastHeight) / 2, 0); //Moves the Grounch check inversely

            controller.Move(slideDirection * slideStrength * speed * Time.deltaTime); //Move them forward at a speed based on the dash strength
        }
        #endregion
    }
    void Jump()
    {
        if (!GameManager.s_Instance.paused && coyoteTime > 0 && hasJumped == false)
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
            isSliding = false;
            prevJump = false;

            jump = true;

            onJump?.Invoke();
            _onJump?.Raise();
        }
    }

    void Crouch()
    {
        if(!GameManager.s_Instance.paused && isGrounded == true)
        {
            if (isCrouching == false)
            {
                if(controls.Player.Movement.ReadValue<Vector2>().y <= 0)
                {
                    isCrouching = true;
                    oldSpeed = speed;
                    speed /= 2;
                }
                else
                {
                    StartCoroutine(EnableDisableSlide());
                }
            }

            else
            {
                isCrouching = false;
                speed = oldSpeed;
            }
        }

    }

    public bool GetIsCrouching()
    {
        return isCrouching;
    }

    public bool GetIsMoving()
    {
        return isMoving;
    }

    public bool GetIsGrounded()
    {
        return isGrounded;
    }

    IEnumerator Dash()
    {
        // Checks:
        // - If the game isn't paused
        // - Not on the ground
        // - Not cooling down
        // - Not already dashing
        // - If the player has enough dashes remaining

        if (!GameManager.s_Instance.paused && isGrounded != true && cooldown == false && isDashing == false && dashesPerformed < dashesBeforeLanding)
        {
            isDashing = true; //Set isDashing to true, which allows the if(dashing is true) statement in Update to start
            dashSliderTime = 0f;
            movementBlocker = true;

            onDash?.Invoke();
            _onDash?.Raise();

            isCrouching = false;
            StartCoroutine(Cooldown());

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

            //timeBeforeDash.value -= 50;
            dashUI.text = ("Dashes: " + (dashesBeforeLanding - dashesPerformed));
        }
    }

    IEnumerator EnableDisableSlide()
    {
        isSliding = true;

        onSlide?.Invoke();
        _onSlide?.Raise();

        yield return new WaitForSeconds(slideTime); //Performs the slide section of update until the set slideTime is up

        onSlideEnd?.Invoke();
        _onSlideEnd?.Raise();

        isSliding = false;

        hasLetGo = false;
        slideDirectionDecided = false;
        cooldown = false;

        if (headIsTouchingSomething) //Keeps the player crouched if they finish their slide underneath a small gap.
        {
            isCrouching = true;
            oldSpeed = speed;
            speed /= 2;
        }
    }

    IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(cooldownTime); //If the cooldown is active, wait for cooldown time set, until setting cooldown as false
        cooldown = false;
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