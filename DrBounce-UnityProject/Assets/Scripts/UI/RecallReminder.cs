using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RecallReminder : MonoBehaviour
{
    [SerializeField] private GameObject Text;
    public UnityEvent OnRecallPrompt = null;
    public UnityEvent OnRecallDone = null;

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
        OnRecallPrompt?.Invoke();
    }

    private void TurnOff()
    {
        Text.SetActive(false);
        OnRecallDone?.Invoke();
    }
}
