using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MouseLook : MonoBehaviour
{
    [Header("Declarations")]
    [SerializeField]
    private Transform playerBody;
    [SerializeField]
    private FloatSetting mouseSensSetting = null;
    [SerializeField]
    private FloatSetting controllerSensSetting = null;

    [Header("Mouse Sensitivity Settings")]
    //[SerializeField]
    private float mouseSensitivityX = 0.0f;
    //[SerializeField]
    private float mouseSensitivityY = 0.0f;

    private float currentMouseSensitivity = 4.0f;

    [Header("Controller Sensitivity Settings")]
    //[SerializeField]
    private float controllerSensitivityX = 0.0f;
    //[SerializeField]
    private float controllerSensitivityY = 0.0f;
    [SerializeField]
    private float controllerAssistSensitivityMultiplier = 0.5f;

    private float currentControllerSensitivity = 7.0f;

    private float xRotation = 0f;

    private float currentControllerSensitivityMultiplier = 1f;

    public Vector2 aimAssistInfluence { private get; set; } = Vector2.zero;

    public InputMaster controls;

    private void Start()
    {
        controls = InputManager.inputMaster;

        Cursor.lockState = CursorLockMode.Locked;

        SetMouseSensitivity(mouseSensSetting.GetCurrentValue());
        SetControllerSensitivity(controllerSensSetting.GetCurrentValue());

        // mouseSensitivityX = mouseSensitivityY = currentMouseSensitivity;
        // controllerSensitivityX = mouseSensitivityY = mouseSensitivity;
        // controllerSensitivityX = currentMouseSensitivity;
        // controllerSensitivityY = currentMouseSensitivity;
    }

    private void LateUpdate()
    {
        if (GameManager.s_Instance.paused) { return; }

        float conX = 0;
        float conY = 0;

        if (Gamepad.current != null)
        {
            conX = Gamepad.current.rightStick.x.ReadValue() * controllerSensitivityX * 50.0f * Time.deltaTime;
            conY = Gamepad.current.rightStick.y.ReadValue() * controllerSensitivityY * 50.0f * Time.deltaTime;

            // Aim assist
            conX *= currentControllerSensitivityMultiplier;
            conY *= currentControllerSensitivityMultiplier;
        }

        float mouseX = (Mouse.current.delta.x.ReadValue() * mouseSensitivityX * 3.5f) * Time.deltaTime;
        float mouseY = (Mouse.current.delta.y.ReadValue() * mouseSensitivityY * 3.5f) * Time.deltaTime;

        float camX = conX + mouseX;
        float camY = conY + mouseY;

        Vector2 moveInput = controls.Player.Movement.ReadValue<Vector2>();

        if((camX == 0 && camY == 0) && (moveInput.x == 0 && moveInput.y == 0))
        {
            aimAssistInfluence = Vector2.zero;
        }
        else if((camX == 0 && camY == 0) || (moveInput.x == 0 && moveInput.y == 0))
        {
            aimAssistInfluence = aimAssistInfluence / 1.5f;
        }

        camX -= aimAssistInfluence.x; camY -= aimAssistInfluence.y;

        xRotation -= camY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
        playerBody.Rotate(Vector3.up * camX);
    }

    public void SetMouseSensitivity(float value)
    {
        currentMouseSensitivity = value;
        mouseSensitivityX = mouseSensitivityY = currentMouseSensitivity;
    }

    public void SetControllerSensitivity(float value)
    {
        currentControllerSensitivity = value;
        controllerSensitivityX = currentControllerSensitivity;
        controllerSensitivityY = currentControllerSensitivity;
    }

    public void IsHovering(bool hover)
    {
        currentControllerSensitivityMultiplier = hover ? controllerAssistSensitivityMultiplier : 1f;
    }
}
