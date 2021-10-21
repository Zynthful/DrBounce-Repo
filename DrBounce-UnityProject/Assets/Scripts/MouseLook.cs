using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MouseLook : MonoBehaviour
{
    public float mouseSensitivity = 4.0f;

    public float mouseSensitivityX = 0;
    public float mouseSensitivityY = 0;

    public float controllerSensitivity = 7.0f;

    public float controllerSensitivityX = 0;
    public float controllerSensitivityY = 0;

    public Transform playerBody;

    float xRotation = 0f;

    // Start is called before the first frame update

    public InputMaster controls;

    private void Awake()
    {
        controls = new InputMaster();
    }
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        // Initialise sensitivity as according to player prefs, or to default if pref does not exist
        SetMouseSensitivity(PlayerPrefs.GetFloat("Options/MouseSensitivity", mouseSensitivity));
        SetControllerSensitivity(PlayerPrefs.GetFloat("Options/ControllerSensitivity", controllerSensitivity));

        mouseSensitivityX = mouseSensitivityY = mouseSensitivity;
        // controllerSensitivityX = mouseSensitivityY = mouseSensitivity;
        controllerSensitivityX = controllerSensitivity;
        controllerSensitivityY = controllerSensitivity;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.s_Instance.paused) { return; }

        float conX = 0;
        float conY = 0;

        if (Gamepad.current != null)
        {
            conX = Gamepad.current.rightStick.x.ReadValue() * controllerSensitivityX * 80.0f * Time.deltaTime;
            conY = Gamepad.current.rightStick.y.ReadValue() * controllerSensitivityY * 80.0f * Time.deltaTime;
        }

        float mouseX = Mouse.current.delta.x.ReadValue() * mouseSensitivityX * 10.0f * Time.deltaTime;
        float mouseY = Mouse.current.delta.y.ReadValue() * mouseSensitivityY * 10.0f * Time.deltaTime;

        float camX = conX + mouseX;
        float camY = conY + mouseY;

        xRotation -= camY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
        playerBody.Rotate(Vector3.up * camX);
    }

    public void SetMouseSensitivity(float value)
    {
        mouseSensitivity = value;
        mouseSensitivityX = mouseSensitivityY = mouseSensitivity;
    }
    public void SetControllerSensitivity(float value)
    {
        controllerSensitivity = value;
        controllerSensitivityX = controllerSensitivity;
        controllerSensitivityY = controllerSensitivity;
    }
}
