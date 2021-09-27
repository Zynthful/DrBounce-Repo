using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using MoreMountains.Feedbacks;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;

    public float speed = 12f;
    public float gravity = -9.81f;
    public float jumpHeight = 3f;

    public float dashStrength = 25f;
    public float dashLength = 0.1f;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    public Vector3 velocity;
    bool isGrounded;

    public InputMaster controls;

    private bool isDashing = false;
    public int dashesBeforeLanding;
    private int dashesPerformed = 0;

    private bool cooldown = false;
    public float cooldownTime = 2;

    private bool isCrouching;

    [Header("Feedbacks")]
    public MMFeedbacks DashFeedback;

    private void Awake()
    {
        controls = new InputMaster(); //Creates a new InputMaster to gain access to mapped controls
        controls.Player.Jump.performed += _ => Jump(); //When the jump action is activated in Input Master, activate the Jump function.
        controls.Player.Dash.performed += _ => StartCoroutine(Dash());
    }
    void Update()
    {
        if(controls.Player.Dash.ReadValue<float>() == 0 && isCrouching == true)
        {
            Debug.Log("HeeHoo, I am not crouching");
            //Add Un-Crouch code
            isCrouching = false;
        }
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask); //Returns true to isGrounded if a small sphere collider below the player overlaps with something with the ground Layer

        if (isGrounded)
        {
            if(dashesPerformed > 0)
            {
                StartCoroutine(StopDash()); //Starts coroutine stopdash, which waits a split second after hitting the ground to reset the dash counter.
            }
            if (velocity.y < 0) //If player is grounded and velocity is lower than 0, set it to 0.
            {
                velocity.y = 0f;
            }
        }

        float x = controls.Player.Movement.ReadValue<Vector2>().x; //Reads the value set from the Input Master based on which keys are being pressed, or where the player is holding on a joystick.
        float z = controls.Player.Movement.ReadValue<Vector2>().y;


        Vector3 move = transform.right * x + transform.forward * z; //Creates a value to move the player in local space based on this value.

        controller.Move(move * speed * Time.deltaTime); //uses move value to move the player.

        velocity.y += gravity * Time.deltaTime; //Raises velocity the longer the player falls for.

        controller.Move(velocity * Time.deltaTime); //Moves the player based on this velocity.


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
        }
    }

    private void Jump()
    {
        if (isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
        }
    }

    IEnumerator Dash()
    {
        if (isGrounded != true) //If the player is on the ground when they've pressed the dash button
        {
            if (cooldown == false) //And if the dash cooldown isn't active
            {
                DashFeedback?.PlayFeedbacks(); //Play feedback
                isDashing = true; //Set isDashing to true, which allows the if(dashing is true) statement in Update to start
                yield return new WaitForSeconds(dashLength); //Continue this if statement every frame for the set dash length
                dashesPerformed += 1;

                isDashing = false;
            }

            else
            {
                yield return new WaitForSeconds(cooldownTime); //If the cooldown is active, wait for cooldown time set, until setting cooldown as false
                cooldown = false;
            }
        }

        else
        {
            print("Heehoo, I am crouching.");
            //Add Crouch Code
            isCrouching = true;
        }

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