using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SetEventSystemSelectedObject : MonoBehaviour
{
    // Currently, this script checks if overrideButton is interactable
    // If so, the event system's first selected object will be set to this button

    // TODO: make this not bad?

    [SerializeField]
    private Button overrideButton = null;

    private void Awake()
    {
        if (overrideButton.interactable)
        {
            EventSystem.current.firstSelectedGameObject = overrideButton.gameObject;
        }
    }
}
