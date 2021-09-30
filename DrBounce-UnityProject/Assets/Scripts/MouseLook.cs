using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MouseLook : MonoBehaviour
{
    public float mouseSensitivity = 100f;

    public float controllerSensitivity = 150f;

    public Transform playerBody;

    float xRotation = 0f;

    float controllerX;
    float controllerY;
    float xRotationC = 0f;

    // Start is called before the first frame update

    public InputMaster controls;

    private void Awake()
    {
        controls = new InputMaster();
    }
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = Mouse.current.delta.x.ReadValue() * mouseSensitivity * Time.deltaTime;
        float MouseY = Mouse.current.delta.y.ReadValue() * mouseSensitivity * Time.deltaTime;

        xRotation -= MouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
        playerBody.Rotate(Vector3.up * mouseX);

        if (Gamepad.current != null)
        {
            float controllerX = Gamepad.current.rightStick.x.ReadValue() * controllerSensitivity * Time.deltaTime;
            float controllerY = Gamepad.current.rightStick.y.ReadValue() * controllerSensitivity * Time.deltaTime;

            xRotationC -= controllerY;
            xRotationC = Mathf.Clamp(xRotationC, -90f, 90f);

            transform.localRotation = Quaternion.Euler(xRotationC, 0, 0);
            playerBody.Rotate(Vector3.up * controllerX);
        }
    }

    public void SetSensitivity(float value)
    {
        mouseSensitivity = value;
    }
}
