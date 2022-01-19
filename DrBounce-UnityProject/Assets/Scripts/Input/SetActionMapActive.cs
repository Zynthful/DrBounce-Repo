using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SetActionMapActive : MonoBehaviour
{
    [SerializeField]
    private string mapName = null;

    public void SetActive(bool value)
    {
        InputActionMap map = InputManager.inputMaster.asset.FindActionMap(mapName);
        InputManager.SetActionMapActive(map, value);
    }

    public void Toggle()
    {
        InputActionMap map = InputManager.inputMaster.asset.FindActionMap(mapName);
        InputManager.SetActionMapActive(map, !map.enabled);
    }
}