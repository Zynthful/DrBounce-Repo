using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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

    Vector3 velocity;
    bool isGrounded;

    private Vector2 test;

    public InputMaster controls;

    private bool isDashing = false;

    private void Awake()
    {
        controls = new InputMaster();
        controls.Player.Jump.performed += _ => Jump();
        controls.Player.Dash.performed += _ => StartCoroutine(Dash());
    }
    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = 0f;
        }

        float x = controls.Player.Movement.ReadValue<Vector2>().x;
        float z = controls.Player.Movement.ReadValue<Vector2>().y;


        Vector3 move = transform.right * x + transform.forward * z;

        controller.Move(move * speed * Time.deltaTime);

        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);

        if(isDashing == true)
        {
            float x2 = controls.Player.Movement.ReadValue<Vector2>().x;
            float z2 = controls.Player.Movement.ReadValue<Vector2>().y;

            Vector3 move2 = transform.right * x + transform.forward * z;

            controller.Move(move2 * dashStrength * speed * Time.deltaTime);
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
        if (isGrounded != true)
        {
            isDashing = true;
            yield return new WaitForSeconds(dashLength);
            isDashing = false;
        }
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