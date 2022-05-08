using UnityEngine;
using UnityEngine.UI;
using Unity.VectorGraphics;

[ExecuteInEditMode]
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
        UpdateUI();
    }

    private void OnValidate()
    {
        UpdateUI();
    }

    private void UpdateUI()
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
            //todo: make it change for keyboard/controller

        }
    }
}