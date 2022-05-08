using UnityEngine;
using UnityEngine.UI;
using Unity.VectorGraphics;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;
using UnityEngine.InputSystem.LowLevel;

public class SetSpriteFromInputAction : MonoBehaviour
{
    [SerializeField]
    [Tooltip("The action to retrieve the input sprite from (M&KB).")]
    private InputActionSetting actionSettingKeyboard;
    [SerializeField]
    [Tooltip("The action to retrieve the input sprite from (Controller).")]
    private InputActionSetting actionSettingController;

    [SerializeField]
    [Tooltip("The SVG image to apply the sprite to.")]
    private SVGImage image;

    private void OnEnable()
    {
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

        if (actionSettingController != null && actionSettingKeyboard == null)
        {
            image.sprite = actionSettingController.GetSprite();
        }
        else if (actionSettingKeyboard != null && actionSettingController == null)
        {
            image.sprite = actionSettingKeyboard.GetSprite();
        }
        else
        {
            // Set the sprite based on whether we're currently using a Gamepad or Mouse/Keyboard
            if (device is Mouse || device is Keyboard)
            {
                image.sprite = actionSettingKeyboard.GetSprite();
            }
            else if (device is Gamepad)
            {
                image.sprite = actionSettingController.GetSprite();
            }
        }
    }
}