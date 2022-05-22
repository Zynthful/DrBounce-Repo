using UnityEngine;
using UnityEngine.UI;
using Unity.VectorGraphics;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.DualShock;

public class SetSpriteFromInputAction : MonoBehaviour
{
    [SerializeField]
    [Tooltip("The action to retrieve the input sprite from (M&KB).")]
    private InputActionSetting actionSettingKeyboard;
    [SerializeField]
    [Tooltip("The action to retrieve the input sprite from (Controller).")]
    private InputActionSetting actionSettingController;

    [SerializeField]
    private bool disableImageIfKeyboardActionNull = true;
    [SerializeField]
    private bool disableImageIfControllerActionNull = true;

    [SerializeField]
    [Tooltip("The SVG image to apply the sprite to.")]
    private SVGImage image;

    private void OnEnable()
    {
        // Initialise w/ gamepad controls if one is plugged in, otherwise try keyboard
        if (Gamepad.current != null)
        {
            UpdateUI(Gamepad.current);
        }
        else if (Keyboard.current != null)
        {
            UpdateUI(Keyboard.current);
        }

        InputUser.listenForUnpairedDeviceActivity = 1;
        InputUser.onUnpairedDeviceUsed += OnUnpairedDeviceUsed;
    }

    private void OnDisable()
    {
        InputUser.onUnpairedDeviceUsed -= OnUnpairedDeviceUsed;
    }

    private void OnUnpairedDeviceUsed(InputControl control, InputEventPtr ptr)
    {
        if (!control.device.noisy)
        {
            UpdateUI(control.device);
        }
    }

    private void UpdateUI(InputDevice device)
    {
        if (image == null)
        {
            Debug.LogWarning($"{name}: An image has not been set for this object.", this);
            return;
        }
        if (actionSettingController == null && actionSettingKeyboard == null)
        {
            Debug.LogWarning($"{name}: No input actions have been set for this object.", this);
            return;
        }

        // Set the sprite based on whether we're currently using a Gamepad or Mouse/Keyboard
        if (device is Mouse || device is Keyboard)
        {
            if (disableImageIfKeyboardActionNull)
                image.enabled = actionSettingKeyboard != null;      // Disable image if we have no keyboard action

            if (actionSettingKeyboard != null)
            {
                image.sprite =
                    InputManager.s_Instance.FindDescription(actionSettingKeyboard.GetEffectivePath()).defaultSprite;
            }
        }
        else if (device is Gamepad)
        {
            if (disableImageIfControllerActionNull)
                image.enabled = actionSettingController != null;    // Disable image if we have no controller action

            if (actionSettingController != null)
            {
                image.sprite = device is DualShockGamepad ?
                    InputManager.s_Instance.FindDescription(actionSettingController.GetEffectivePath()).dualshockSprite :
                    InputManager.s_Instance.FindDescription(actionSettingController.GetEffectivePath()).defaultSprite;
            }
        }
    }
}