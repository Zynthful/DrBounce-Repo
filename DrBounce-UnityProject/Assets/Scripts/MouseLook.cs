using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MouseLook : MonoBehaviour
{
    [Header("Declarations")]
    [SerializeField]
    private Transform playerBody;

    [Header("Mouse Sensitivity Settings")]
    [SerializeField]
    private float defaultMouseSensitivity = 4.0f;
    //[SerializeField]
    private float mouseSensitivityX = 0.0f;
    //[SerializeField]
    private float mouseSensitivityY = 0.0f;

    private float currentMouseSensitivity = 4.0f;

    [Header("Controller Sensitivity Settings")]
    [SerializeField]
    private float defaultControllerSensitivity = 7.0f;
    //[SerializeField]
    private float controllerSensitivityX = 0.0f;
    //[SerializeField]
    private float controllerSensitivityY = 0.0f;
    [SerializeField]
    private float controllerAssistSensitivityMultiplier = 0.5f;

    private float currentControllerSensitivity = 7.0f;

    private float xRotation = 0f;

    private float currentControllerSensitivityMultiplier = 1f;

    // Start is called before the first frame update

    public InputMaster controls;

    private void Awake()
    {
        controls = new InputMaster();
    }
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        // Initialise sensitivity as according to player prefs, or to default if pref does not exist
        SetMouseSensitivity(PlayerPrefs.GetFloat("Options/Sensitivity/MouseSensitivity", defaultMouseSensitivity));
        SetControllerSensitivity(PlayerPrefs.GetFloat("Options/Sensitivity/ControllerSensitivity", defaultControllerSensitivity));

        // mouseSensitivityX = mouseSensitivityY = currentMouseSensitivity;
        // controllerSensitivityX = mouseSensitivityY = mouseSensitivity;
        // controllerSensitivityX = currentMouseSensitivity;
        // controllerSensitivityY = currentMouseSensitivity;
    }

    // Update is called once per frame
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

        float mouseX = Mouse.current.delta.x.ReadValue() * mouseSensitivityX * 3.5f * Time.deltaTime;
        float mouseY = Mouse.current.delta.y.ReadValue() * mouseSensitivityY * 3.5f * Time.deltaTime;

        float camX = conX + mouseX;
        float camY = conY + mouseY;

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
