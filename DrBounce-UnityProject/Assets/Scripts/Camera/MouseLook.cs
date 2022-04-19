using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MouseLook : MonoBehaviour
{
    [Header("Declarations")]
    [SerializeField]
    private Transform playerBody;

    [Header("Sensitivity Settings")]
    [SerializeField]
    private FloatSetting mouseSensitivity = null;
    [SerializeField]
    private FloatSetting controllerSensitivity = null;
    [SerializeField]
    private float controllerAssistSensitivityMultiplier = 0.5f;

    private float xRotation = 0f;

    public Vector2 aimAssistInfluence { private get; set; } = Vector2.zero;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void LateUpdate()
    {
        if (GameManager.s_Instance.paused) { return; }

        float conX = 0;
        float conY = 0;

        if (Gamepad.current != null)
        {
            conX = Gamepad.current.rightStick.x.ReadValue() * controllerSensitivity.GetCurrentValue() * 25.0f * Time.deltaTime;
            conY = Gamepad.current.rightStick.y.ReadValue() * controllerSensitivity.GetCurrentValue() * 25.0f * Time.deltaTime;

            // Old Aim assist
            //conX *= currentControllerSensitivityMultiplier;
            //conY *= currentControllerSensitivityMultiplier;
        }

        float mouseX = Mouse.current.delta.x.ReadValue() * mouseSensitivity.GetCurrentValue() * 0.01f;
        float mouseY = Mouse.current.delta.y.ReadValue() * mouseSensitivity.GetCurrentValue() * 0.01f;

        float camX = conX + mouseX;
        float camY = conY + mouseY;

        Vector2 moveInput = InputManager.inputMaster.Player.Movement.ReadValue<Vector2>();

        if((camX == 0 && camY == 0) && (moveInput.x == 0 && moveInput.y == 0))
        {
            aimAssistInfluence = Vector2.zero;
        }
        else if((Mathf.Sign(aimAssistInfluence.x) == Mathf.Sign(camX)) || (Mathf.Sign(aimAssistInfluence.y) == Mathf.Sign(camY)))
        {
            aimAssistInfluence = aimAssistInfluence / 2.25f;
        }
        else if((camX == 0 && camY == 0) || (moveInput.x == 0 && moveInput.y == 0))
        {
            aimAssistInfluence = aimAssistInfluence / 1.5f;
        }

        aimAssistInfluence /= controllerSensitivity.GetCurrentValue();

        camX -= aimAssistInfluence.x; camY -= aimAssistInfluence.y;

        xRotation -= camY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
        playerBody.Rotate(Vector3.up * camX);
    }

    public void IsHovering(bool hover)
    {
        //currentControllerSensitivityMultiplier = hover ? controllerAssistSensitivityMultiplier : 1f;
    }
}