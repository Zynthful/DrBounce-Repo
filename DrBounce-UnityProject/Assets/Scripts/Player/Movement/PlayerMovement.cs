using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;
    public static PlayerMovement instance;

    private bool isCrouching;
    private CharacterController charController;
    private float playerHeight;
    private float oldSpeed;

    [Header("Base Movement")]
    [SerializeField] private float speed = 8f;
    [SerializeField] private float gravity = -19.81f;
    public static Transform player;
    public InputMaster controls;
    [SerializeField] private Vector3 move;
    [Tooltip("The higher the number, the quicker your momentum dies. 0 depletes it super slowly")]
    public float momentumLossRate;
    [SerializeField] private float momentumStrength = 5;
    [SerializeField] private float acceleration;
    [SerializeField] private float accelerationSpeed;
    private bool isMoving = false;
    [HideInInspector] public Vector3 bounceForce;
    private bool hasMoved = false;
    private Vector3 oldMove;

    [Header("Jump")]
    [SerializeField] private float jumpPeak = 3f;
    [SerializeField] private float jumpMin = 1f;
    [Tooltip("The higher the value, the heavier the player is.")]
    [SerializeField] private float floatiness;
    [Tooltip("Set between 1 and 0, with 1 being lots of time and 0 being none")]

    [SerializeField] private float coyoteTime;
    private float oldCoyoteTime;
    private bool jump = false;
    private float jumpHeight = 0f;
    private bool prevJump = false;
    private float prevGrav;
    private bool hasJumped = false;

    [Header("Dashing")]
    [SerializeField] private float dashStrength = 8;
    [SerializeField] private float dashLength = 0.2f;
    [SerializeField] private int dashesBeforeLanding;
    [SerializeField] private float cooldownTime = 0.5f;
    [SerializeField] private float extendedNoGravTime = 0.1f;
    [SerializeField] private bool cooldown = false;
    private Vector3 dashDirection;
    [HideInInspector] public bool isDashing = false;
    private int dashesPerformed = 0;
    private bool dashLocker = false;

    private float dashSliderTime = 0f;

    [Header("Sliding")]
    [SerializeField] private float slideTime;
    [SerializeField] private float slideStrength;
    public bool isSliding = false;
    [SerializeField] private float strafeStrength;
    [SerializeField] private float slideGravity;
    private bool slideDirectionDecided = false;
    [HideInInspector] public Vector3 slideDirection;
    private Vector3 slideLeftRight;
    private bool headCheckPerformed = false;
    public bool canSlide = true;

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
    public float slopeSensitivity = 0.4f;
    [HideInInspector] public bool isGrounded;
    [HideInInspector] public bool headIsTouchingSomething;
    public Vector3 velocity;
    private bool slopeCheck;
    private Vector3 headCheckHeight;
    [HideInInspector] public Vector3 groundcheckPos;

    [Header("Freeze")]
    [SerializeField] private bool Freeze = false;

    [Header("Unity Events")]
    public UnityEvent onJump = null;
    public UnityEvent onDash = null;
    public UnityEvent onSlide = null;
    public UnityEvent onSlideEnd = null;
    public UnityEvent onLand = null;
    public UnityEvent onLandOnNonBounceableGround = null;
    public UnityEvent onCrouch = null;
    public UnityEvent onUncrouch = null;

    [Header("Game Events")]
    public GameEventFloat onDashSliderValue = null;

    //private Collider[] test;

    private void Awake()
    {
        oldCoyoteTime = coyoteTime;
        prevGrav = gravity;
        charController = GetComponent<CharacterController>();
        playerHeight = charController.height;
        GameManager.gravity = gravity;

        controls = InputManager.inputMaster;

        instance = this;
        player = transform;
        headCheckHeight = new Vector3(0.25f, 0.15F, 0.25f);
    }

    private void Start()
    {
        if (Freeze)
        {
            controls.Player.Movement.Disable();
            controls.Player.Jump.Disable();
            controls.Player.Crouch.Disable();
        }
    }

    public static void SetMovementActive(bool active)
    {
        if (active)
        {
            InputManager.inputMaster.Player.Movement.Enable();
            InputManager.inputMaster.Player.Jump.Enable();
            InputManager.inputMaster.Player.Crouch.Enable();
        }
        else
        {
            InputManager.inputMaster.Player.Movement.Disable();
            InputManager.inputMaster.Player.Jump.Disable();
            InputManager.inputMaster.Player.Crouch.Disable();
        }
    }

    #region BrackeysMoment
    private void OnEnable()
    {
        // Listen for player inputs
        controls.Player.Jump.performed += _ => Jump();
        controls.Player.Dash.performed += _ => InitiateDash();
        controls.Player.Crouch.performed += _ => Crouch();
    }

    private void OnDestroy()
    {
        controls.Player.Jump.performed -= _ => Jump();
        controls.Player.Dash.performed -= _ => InitiateDash();
        controls.Player.Crouch.performed -= _ => Crouch();
    }

    private void OnDisable()//Brackeys Moment   // brackeys is gone :(
    {
        controls.Player.Jump.performed -= _ => Jump();
        controls.Player.Dash.performed -= _ => InitiateDash();
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

        #region Movement
        if (!GameManager.s_Instance.paused && isDashing == false)
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
                oldMove = move * speed;
                move = (transform.right * x + transform.forward * z).normalized * acceleration; //Creates a value to move the player in local space based on this value.
                controller.Move(move * speed * Time.deltaTime); //uses move value to move the player.
                velocity -= (((move * speed) - (oldMove)) * 0.5f);
            }
            else
            {
                move = (slideLeftRight * x); //Creates a value to move the player in local space based on this value.
                controller.Move(move * strafeStrength * Time.deltaTime); //uses move value to move the player.
            }

            // Check if moving
            isMoving = move != Vector3.zero ? true : false;
        }

        if (move == new Vector3(0,0,0))
        {
            //print(move);
            hasMoved = false;
        }

        if (controls.Player.Movement.ReadValue<Vector2>().x == 0 && controls.Player.Movement.ReadValue<Vector2>().y == 0)
        {
            acceleration = 0;
        }


        #endregion

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
            headCheckHeight -= new Vector3(0, (charController.height - lastHeight) / 2, 0);
        }
        #endregion

        #region Slide

        if (isSliding == true)
        {
            knockbackPower = 0;
            acceleration = 1;
            if (slideDirectionDecided == false)
            {
                slideDirectionDecided = true;
                slideDirection = transform.forward;
                slideLeftRight = transform.right;
                velocity.x = (slideDirection.x * slideStrength) * 1.5f; //Move them forward at a speed based on the dash strength
                velocity.z = (slideDirection.z * slideStrength) * 1.5f; //Move them forward at a speed based on the dash strength
            }
            controller.Move(slideDirection * slideStrength * Time.deltaTime);
            h = playerHeight * 0.35f;
            float lastHeight = charController.height;
            //Moves the player downward
            charController.height = Mathf.Lerp(charController.height, h, 20 * Time.deltaTime);
            transform.localPosition += new Vector3(0, (charController.height - lastHeight) / 2, 0);
            headCheckHeight -= new Vector3(0, (charController.height - lastHeight) / 2, 0);
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

        #region Dashing
        if (isDashing == true)
        {
            //Resets velocity and sets Y velocity to 0 while dashing so the player stays at the same elevation for the duration of the move
            velocity = Vector3.zero;
            knockbackPower = 0;
            cooldown = true;
            //Locks in the direction of the dash once the player's directional input has been read so it can't be changed mid-dash
            if (dashLocker == false)
            {
                dashLocker = true;
                float dashX = controls.Player.Movement.ReadValue<Vector2>().x;
                float dashZ = controls.Player.Movement.ReadValue<Vector2>().y;
                //If the player dashes without a directional input, it will default to moving the player straight forward.

                if (dashX == 0 && dashZ == 0)
                {
                    dashDirection = transform.forward;
                }
                //If there is a direction input before dashing, dash in that direction
                else
                {
                    dashDirection = (transform.right * dashX + transform.forward * dashZ).normalized;
                }
            }
            //Moves the player in the given dash direction
            controller.Move(dashDirection * dashStrength * Time.deltaTime);
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

        #region Knockback

        if (knockbackPower > 0)
        {
            controller.Move(knockbackDir * knockbackPower * Time.deltaTime); //Move them in a direction at a speed based on the knockback strength
            knockbackPower -= Time.deltaTime * knockbackDecayMultiplier;
        }

        #endregion

        #region Momentum

        //Allows the player to push against their momentum to slow it down without springing back after letting go
        //This is accomplished by subtracting the player's input value 'move' from the player's velocity when they're in opposite directions
        if (velocity.x > 0 && move.x < 0 || velocity.x < 0 && move.x > 0)
        {
            velocity.x += move.x;
        }
        if (velocity.z > 0 && move.z < 0 || velocity.z < 0 && move.z > 0)
        {
            velocity.z += move.z;
        }

        controller.Move(new Vector3(Mathf.Abs(charController.velocity.x + velocity.x + bounceForce.x) * velocity.x / (10 / (0.1f * momentumStrength)),
            velocity.y,
            Mathf.Abs(charController.velocity.z + velocity.z + bounceForce.z) * velocity.z / (10 / (0.1f * momentumStrength))) * Time.deltaTime);

        //if (gameObject.GetComponent<CharacterController>().velocity.x == 0 && bounceForce.x == 0)
        //{
        //    velocity.x = 0;
        //}

        //if (gameObject.GetComponent<CharacterController>().velocity.z == 0 && bounceForce.z == 0)
        //{
        //    velocity.z = 0;
        //}

        #endregion

        #region GroundChecking
        bool wasGrounded = isGrounded;


        //CUBE DEBUGGING COMMENTED OUT BELOW - PLACES CUBES THAT MIMIC THE PLAYER'S GROUNDCHECK BOX, SLOPECHECK BOX AND HEADCHECK BOX RESPECTIVELY.

        groundcheckPos = new Vector3(transform.position.x, transform.position.y - (charController.height / 2), transform.position.z);

        //GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        //cube.transform.position = groundcheckPos;
        //cube.transform.rotation = transform.rotation;
        //cube.transform.localScale = new Vector3(0.2f, 0.2F, 0.2f) * 2;
        //cube.GetComponent<Collider>().enabled = false;
        //cube.GetComponent<Renderer>().material.color = Color.green;

        //GameObject cube2 = GameObject.CreatePrimitive(PrimitiveType.Cube);
        //cube2.transform.position = groundcheckPos + move + (Vector3.down / 2.5f);
        //cube2.transform.rotation = transform.rotation;
        //cube2.transform.localScale = new Vector3(0.01f, slopeSensitivity, 0.01f) * 2;
        //cube2.GetComponent<Collider>().enabled = false;
        //cube2.GetComponent<Renderer>().material.color = Color.red;

        //GameObject cube3 = GameObject.CreatePrimitive(PrimitiveType.Cube);
        //cube3.transform.position = new Vector3(transform.position.x, transform.position.y + (charController.height / 2) + headCheckHeight.y, transform.position.z);
        //cube3.transform.rotation = transform.rotation;
        //cube3.transform.localScale = headCheckHeight * 2;
        //cube3.GetComponent<Collider>().enabled = false;
        //cube3.GetComponent<Renderer>().material.color = Color.blue;
        //Returns true to isGrounded if a small cube collider below the player overlaps with something with the ground Layer

        //A wider checkbox for isGrounded helps with slope detection, but too large allows player to jump off of walls.
        isGrounded = Physics.CheckBox(groundcheckPos, new Vector3(0.2f, 0.2F, 0.2f), transform.rotation, ~groundMask);
        headIsTouchingSomething = Physics.CheckBox(new Vector3(transform.position.x, transform.position.y + (charController.height / 2) + headCheckHeight.y, transform.position.z), headCheckHeight, transform.rotation, ~headMask);
        slopeCheck = Physics.CheckBox(groundcheckPos + move + (Vector3.down / 2.5f), new Vector3(0.01f, slopeSensitivity, 0.01f), transform.rotation, ~groundMask);

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
            }

            //If the player has movement velocity and isn't sliding
            if ((velocity.z != 0 || velocity.x != 0) && isSliding == false)
            {
                // reduce the velocity over time by the momentum loss rate.
                //If the player is moving with the momentum, it won't be depleted.
                //Move is always between 0 & 1 - if the player's movement is at its max,
                //then the full momentum loss rate will be subtracted from itself, making the momentum loss very low.
                velocity.x -= ((velocity.normalized.x * momentumLossRate) - ((move.normalized.x * momentumLossRate / 2))) * Time.deltaTime;
                velocity.z -= ((velocity.normalized.z * momentumLossRate) - ((move.normalized.z * momentumLossRate / 2))) * Time.deltaTime;
            }

            else
            {
                gravity = prevGrav;
                bounceForce = Vector3.zero;
            }

            if (slopeCheck)
            {
                //The heavier the gravity value here, the better the player will stick to slopes when walking or sliding down them.
                velocity.y = -1000;
            }
        }
        else
        {
            Gravity();
        }

        if (headIsTouchingSomething && !isGrounded)
        {
            Gravity();

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
        }
    }

    void InitiateDash()
    {
        StartCoroutine(Dash());
    }

    void Land()
    {
        onLand?.Invoke();

        // Is the ground we landed on NOT bounceable?
        if (!Physics.CheckSphere(groundCheck.position, groundDistance, bounceableMask))
        {
            onLandOnNonBounceableGround?.Invoke();
        }
    }

    void Crouch()
    {
        if (GameManager.s_Instance.paused || !isGrounded)
            return;

        if (!isCrouching)
        {
            // Crouch
            if (controls.Player.Movement.ReadValue<Vector2>().y <= 0 || !canSlide)
            {
                isCrouching = true;
                oldSpeed = speed;
                speed /= 2;
                onCrouch.Invoke();
            }
            // Slide
            else if (canSlide)
            {
                isSliding = true;

                // Trigger used unlock for the first time, if it's the first time we've done so
                if (UnlockTracker.instance.lastUnlock == UnlockTracker.UnlockTypes.Slide && !UnlockTracker.instance.usedUnlock)
                {
                    UnlockTracker.instance.UsedUnlockFirstTime();
                }

                onSlide.Invoke();
            }
        }
        // Uncrouch
        else
        {
            onUncrouch.Invoke();
            isCrouching = false;
            speed = oldSpeed;
        }
    }

    public void ApplyKnockback(Vector3 dir, float power)
    {
        knockbackDir = (knockbackDir + dir).normalized;
        knockbackPower = power;
    }

    public void UpdateDashCount(int newAmount)
    {
        dashesBeforeLanding = newAmount;
    }

    public void UpdateCanSlide(bool i_canSlide)
    {
        canSlide = i_canSlide;
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
            // Trigger used unlock for the first time, if it's the first time we've done so
            if ((UnlockTracker.instance.lastUnlock == UnlockTracker.UnlockTypes.FirstDash || UnlockTracker.instance.lastUnlock == UnlockTracker.UnlockTypes.SecondDash) && !UnlockTracker.instance.usedUnlock)
            {
                UnlockTracker.instance.UsedUnlockFirstTime();
            }

            isDashing = true; //Set isDashing to true, which allows the if(dashing is true) statement in Update to start
            dashSliderTime = 0f;

            onDash?.Invoke();

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

            yield return new WaitForSeconds(extendedNoGravTime);
            gravity = oldGravity;
        }
    }

    void Gravity()
    {
        velocity.y += gravity * Time.deltaTime; //Raises velocity the longer the player falls for.
    }

    void DisableSlide()
    {
        onSlideEnd?.Invoke();

        isSliding = false;
        headCheckPerformed = false;
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