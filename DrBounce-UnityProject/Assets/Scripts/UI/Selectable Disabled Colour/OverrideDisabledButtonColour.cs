using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// TODO: Make re-enabling buttons not clunky
public class OverrideDisabledButtonColour : MonoBehaviour
{
    [SerializeField]
    private Button button = null;

    [SerializeField]
    private Graphic disabledGraphic = null;

    [SerializeField]
    private ColorBlock disabledColors = new ColorBlock();

    private bool hasOverridden = false;

    // This is stored so it is restored if the button is made interactable again
    private Graphic enabledGraphic = null;
    private ColorBlock enabledColors;

    private void Awake()
    {
        enabledGraphic = button.targetGraphic;
        enabledColors = button.colors;
        
        if (button == null)
        {
            button = GetComponent<Button>();
            if (button == null)
            {
                Debug.LogError("No button set or detected on this object", this);
            }
        }
    }

    private void OnEnable()
    {
        Override();
    }

    private void Override()
    {
        if (!button.interactable)
        {
            // Disable old enabled graphic if it doesn't match the one to override
            if (button.targetGraphic != disabledGraphic)
            {
                button.targetGraphic.enabled = false;
            }

            // Override with new graphic and colour
            button.targetGraphic = disabledGraphic;
            button.colors = disabledColors;

            hasOverridden = true;
        }
        // Only apply old enabled graphic and colours if it has already been overridden
        else if (hasOverridden)
        {
            button.targetGraphic = enabledGraphic;
            button.targetGraphic.enabled = true;
            button.colors = enabledColors;
        }
    }
}
