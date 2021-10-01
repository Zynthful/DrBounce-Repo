using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using MoreMountains.Feedbacks;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;

    [Header("Base Movement")]
    public float speed = 12f;
    public float gravity = -9.81f;
    public float jumpHeight = 3f;
    public static Transform player;
    public InputMaster controls;

    [Header("Dashing")]
    public float dashStrength = 25f;
    public float dashLength = 0.1f;
    public int dashesBeforeLanding;
    public float cooldownTime = 2;
    private bool cooldown = false;
    private bool isDashing = false;
    private int dashesPerformed = 0;

    [Header("Ground Checking")]
    public Transform groundCheck;
    public LayerMask groundMask;
    public float groundDistance = 0.4f;
    [HideInInspector] public bool isGrounded;
    public Vector3 velocity;

    [Header("Feedbacks")]
    public MMFeedbacks DashFeedback;

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
        charController = GetComponent<CharacterController>();
        playerHeight = charController.height;

        controls = new InputMaster(); //Creates a new InputMaster to gain access to mapped controls
        controls.Player.Jump.performed += _ => Jump(); //When the jump action is activated in Input Master, activate the Jump function.
        controls.Player.Dash.performed += _ => Dash();
        controls.Player.Crouch.performed += _ => Crouch();
        player = transform;
    }
    void Update()
    {
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
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask); //Returns true to isGrounded if a small sphere collider below the player overlaps with something with the ground Layer

        #region DashStopping
        if (isGrounded)
        {
            isDashing = false;
            dashesPerformed = 0;
            if (dashesPerformed > 0)
            {
                StartCoroutine(StopDash()); //Starts coroutine stopdash, which waits a split second after hitting the ground to reset the dash counter.
            } //This delay is to prevent the player being able to dash just before they hit the ground.
            if (velocity.y < 0) //If player is grounded and velocity is lower than 0, set it to 0.
            {
                velocity.y = -2f;
            }
        }
        #endregion

        #region Movement
        float x = controls.Player.Movement.ReadValue<Vector2>().x; //Reads the value set from the Input Master based on which keys are being pressed, or where the player is holding on a joystick.
        float z = controls.Player.Movement.ReadValue<Vector2>().y;


        Vector3 move = (transform.right * x + transform.forward * z).normalized; //Creates a value to move the player in local space based on this value.
        controller.Move(move * speed * Time.deltaTime); //uses move value to move the player.
        velocity.y += gravity * Time.deltaTime; //Raises velocity the longer the player falls for.
        controller.Move(velocity * Time.deltaTime); //Moves the player based on this velocity.
        #endregion

        #region Dashing
        if (isDashing == true && dashesPerformed < dashesBeforeLanding)
        {
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
    }

    private void Jump()
    {
        if (isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);

            onJump?.Raise();
        }
    }

    void Dash()
    {
        isCrouching = false;
        if (isGrounded != true && cooldown == false && isDashing == false) //If the player isn't on the ground, if they're not cooling down, and if they're not already dashing.
        {
            StartCoroutine(CoolDownTest());

            onDash?.Raise();

            StartCoroutine(EnableDisableDash());
        }
    }

    void Crouch()
    {
        if(isGrounded == true)
        {
            if (isCrouching == true)
            {
                print("Heehoo, I am no longer a crouching");
                isCrouching = false;
                speed = oldSpeed;
            }
            else
            {
                print("Heehoo, I am a crouching.");
                isCrouching = true;
                oldSpeed = speed;
                speed /= 2;
            }
        }
    }

    IEnumerator EnableDisableDash()
    {
        isDashing = true; //Set isDashing to true, which allows the if(dashing is true) statement in Update to start

        float oldGravity = gravity;
        gravity = 0;
        DashFeedback?.PlayFeedbacks(); //Play feedback

        yield return new WaitForSeconds(dashLength); //Continue this if statement every frame for the set dash length

        gravity = oldGravity;
        dashesPerformed += 1;

        isDashing = false;
    }

    IEnumerator CoolDownTest()
    {
        yield return new WaitForSeconds(cooldownTime); //If the cooldown is active, wait for cooldown time set, until setting cooldown as false
        cooldown = false;
    }
    IEnumerator StopDash()
    {
        yield return new WaitForSeconds(0.1f);
        dashesPerformed = 0;
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