/// This component is used to update a given selectable's interactable state based on whether a game save exists or not.
/// If a game save exists, it is made interactable, otherwise it is made non-interactable.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveEnableSelectable : MonoBehaviour
{
    [SerializeField]
    private Selectable selectable = null;

    private void Awake()
    {
        if (selectable == null)
        {
            selectable = GetComponent<Selectable>();
        }
    }

    private void OnEnable()
    {
        GameSaveData data = SaveSystem.LoadGameData();
        selectable.interactable = data != null;
    }
}