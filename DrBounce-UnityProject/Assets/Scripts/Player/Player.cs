using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private void Awake()
    {
        GameManager.player = this;
    }

    public static Player GetPlayer()
    {
        return GameManager.player;
    }

    public static void SetControlsActive(bool active)
    {
        if (active)
        {
            InputManager.inputMaster.Player.Enable();
        }
        else
        {
            InputManager.inputMaster.Player.Disable();
        }
    }
}
