using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;
    public static PlayerMovement instance;

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
    private bool headCheckPerformed = false;
    private bool hasLetGo = false;

    // Knockback values
    private float knockbackPower;
    private Vector3 knockbackDir;
    private int knockbackDecayMultiplier = 8;

    [Header("Ground+Head Checking")]
    public Transform groundCheck;
    public Transform headCheck;
    public LayerMask groundMask;
    public LayerMask bounceableMask;
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
    [SerializeField]
    private UnityEvent onLand = null;
    [SerializeField]
    private UnityEvent onLandOnNonBounceableGround = null;

    [Header("Game Events")]
    [SerializeField]
    private GameEvent _onJump = null;
    [SerializeField]
    private GameEvent _onDash = null;
    [SerializeField]
    private GameEvent _onSlide = null;
    [SerializeField]
    private GameEvent _onSlideEnd = null;
    [SerializeField]
    private GameEventFloat onDashSliderValue = null;
    [SerializeField]
    private GameEvent _onLandOnNonBounceableGround = null;

    private void Awake()
    {
        oldCoyoteTime = coyoteTime;
        oldGroundDistance = groundDistance;
        prevGrav = gravity;
        charController = GetComponent<CharacterController>();
        playerHeight = charController.height;
        GameManager.gravity = gravity;

        controls = InputManager.inputMaster;

        instance = this;
        player = transform;
    }

    #region BrackeysMoment
    private void OnEnable()
    {
        // Listen for player inputs
        controls.Player.Jump.performed += _ => Jump();
        controls.Player.Dash.performed += _ => StartCoroutine(Dash());
        controls.Player.Crouch.performed += _ => Crouch();
    }

    private void OnDisable()//Brackeys Moment   // brackeys is gone :(
    {
        controls.Player.Jump.performed -= _ => Jump();
        controls.Player.Dash.performed -= _ => StartCoroutine(Dash());
        controls.Player.Crouch.performed -= _ => Crouch();
    }
    #endregion

    private void FixedUpdate()
    {
        //@cole :)
        if (isDashing == true)
        {
            dashSliderTime += Time.deltaTime;

            float b = (1f - ((float)dashesPerformed / (float)dashesBeforeLanding));
            float a = (1f - (((float)dashesPerformed + 1f) / (float)dashesBeforeLanding));
            float t = 1f - Mathf.Clamp(dashSliderTime / dashLength, 0, 1);
            float dashSliderPos = Mathf.Lerp(a, b, Mathf.SmoothStep(0, 1, t));

            onDashSliderValue?.Raise(dashSliderPos);
        }

        print(move + "move");
        #region Crouching
        //print(isCrouching);
        float h = playerHeight;
        if (isCrouching == true) //If dash button is being held down, and the isCrouching is enabled by the dash coroutine
        {
            h = playerHeight * 0.35f;
        }
        if (isSliding == false)
        {
            float lastHeight = charController.height;
            charController.height = Mathf.Lerp(charController.height, h, 5 * Time.deltaTime);

            //If crouching height is close enough to its target number (with a threshold of 0.05), then set it to that number
            if (((charController.height - h) < 0 ? ((charController.height - h) * -1) : (charController.height - h)) <= 0.05)
            {
                charController.height = h;
            }

            transform.localPosition += new Vector3(0, (charController.height - lastHeight) / 2, 0);
            groundCheck.transform.localPosition -= new Vector3(0, (charController.height - lastHeight) / 2, 0); //Moves the Grounch check inversely
        }

        #endregion

        #region GroundChecking
        bool wasGrounded = isGrounded;

        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, ~groundMask); //Returns true to isGrounded if a small sphere collider below the player overlaps with something with the ground Layer
        headIsTouchingSomething = Physics.CheckSphere(headCheck.position, headDistance, ~headMask);

        coyoteTime -= Time.deltaTime;

        if (isGrounded)
        {
            coyoteTime = oldCoyoteTime;
            hasJumped = false;
            dashesPerformed = 0;

            onDashSliderValue?.Raise(100);

            if (dashesPerformed > 0)
            {
                StartCoroutine(StopDash()); //Starts coroutine stopdash, which waits a split second after hitting the ground to reset the dash counter.
            } //This delay is to prevent the player being able to dash just before they hit the ground.
            if (velocity.y < 0) //If player is grounded and velocity is lower than 0, set it to 0.
            {
                velocity.y = (-40f * Time.fixedDeltaTime);
                //velocity.x = 0;
                //velocity.z = 0;
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

        // Check if we've just become grounded
        if (!wasGrounded && isGrounded)
        {
            Land();
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

        velocity.y += gravity * Time.deltaTime; //Raises velocity the longer the player falls for.
        controller.Move(velocity * Time.deltaTime); //Moves the player based on this velocity.
        #endregion

        #region Dashing
        if (isDashing == true)
        {
            velocity = Vector3.zero;
            knockbackPower = 0;

            if (hasDashed == false)
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
                controller.Move(move * dashStrength * 8 * Time.deltaTime); //Move them forward at a speed based on the dash strength
                hasDashed = true;
                controls.Player.Movement.Disable();
            }
        }
        #endregion

        #region Jumping

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
                //print("Midhop");
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
            knockbackPower = 0;
            acceleration = 1;
            coyoteTime = oldCoyoteTime;
            gravity = slideGravity;
            //print(isGrounded);
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

            velocity = (slideDirection * slideStrength * 8); //Move them forward at a speed based on the dash strength
        }

        if (controls.Player.Crouch.ReadValue<float>() == 0 && isSliding == true) //Stops the player from Sliding
        {
            DisableSlide();

            if (headIsTouchingSomething && headCheckPerformed == false) //Keeps the player crouched if they finish their slide underneath a small gap.
            {
                headCheckPerformed = true;
                isCrouching = true;
                oldSpeed = speed;
                speed /= 2;
            }
        }

        #endregion

        #region Knockback

        if (knockbackPower > 0)
        {
            controller.Move(knockbackDir * knockbackPower * Time.deltaTime); //Move them in a direction at a speed based on the knockback strength
            knockbackPower -= Time.deltaTime * knockbackDecayMultiplier;
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
            if(isSliding == true)
            {
                DisableSlide();
            }
            prevJump = false;

            jump = true;

            onJump?.Invoke();
            _onJump?.Raise();
        }
    }

    void Land()
    {
        onLand?.Invoke();

        // Is the ground we landed on NOT bounceable?
        if (!Physics.CheckSphere(groundCheck.position, groundDistance, bounceableMask))
        {
            onLandOnNonBounceableGround?.Invoke();
            _onLandOnNonBounceableGround?.Raise();
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
                    isSliding = true;

                    onSlide?.Invoke();
                    _onSlide?.Raise();
                }
            }

            else
            {
                isCrouching = false;
                speed = oldSpeed;
            }
        }

    }

    public void ApplyKnockback(Vector3 dir, float power)
    {
        knockbackDir = (knockbackDir + dir).normalized;
        knockbackPower = power;
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
        }
    }

    void DisableSlide()
    {
        onSlideEnd?.Invoke();
        _onSlideEnd?.Raise();

        isSliding = false;

        hasLetGo = false;
        slideDirectionDecided = false;
        cooldown = false;
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

    public bool GetIsCrouching() { return isCrouching; }

    public bool GetIsMoving() { return isMoving; }

    public bool GetIsGrounded() { return isGrounded; }
}