using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecallReminder : MonoBehaviour
{
    [SerializeField] private GameObject Text;

    private void Start()
    {
        TurnOff();
    }

    void OnEnable()
    {
        GunThrowing.OnLeftGun += TurnOn;
        GunThrowing.OnPickedUpGun += TurnOff;
    }


    void OnDisable()
    {
        GunThrowing.OnLeftGun -= TurnOn;
        GunThrowing.OnPickedUpGun -= TurnOff;
    }

    private void TurnOn()
    {
        Text.SetActive(true);
    }

    private void TurnOff()
    {
        Text.SetActive(false);
    }
}
