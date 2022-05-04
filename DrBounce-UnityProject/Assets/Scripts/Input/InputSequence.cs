using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class InputSequence : MonoBehaviour
{

    Coroutine timer;

    public InputMaster controls;

    private int sequenceIndex;

    [SerializeField] private UnityEvent onSequence;


    private void Start()
    {
        controls = new InputMaster();
        controls = InputManager.inputMaster;
    }

    private void Update()
    {
        switch (sequenceIndex)
        {
            case 0:
                if (controls.Keypress.L_Pressed.triggered)
                {
                    sequenceIndex++;
                    if(timer != null)
                        StopCoroutine(timer);
                    timer = StartCoroutine(Timer());
                }
                break;
            case 1:
                if (controls.Keypress.Y_Pressed.triggered)
                {
                    sequenceIndex++;
                    StopCoroutine(timer);
                    timer = StartCoroutine(Timer());
                }
                break;
            case 2:
                if (controls.Keypress.S_Pressed.triggered)
                {
                    sequenceIndex++;
                    StopCoroutine(timer);
                    timer = StartCoroutine(Timer());
                }
                break;
            case 3:
                if (controls.Keypress.U_Pressed.triggered)
                {
                    sequenceIndex++;
                    StopCoroutine(timer);
                    timer = StartCoroutine(Timer());
                }
                break;
            case 4:
                if (controls.Keypress.S_Pressed.triggered)
                {
                    StopCoroutine(timer);
                    sequenceIndex = 0;
                    onSequence?.Invoke();
                }
                break;
        }
    }

    IEnumerator Timer()
    {
        yield return new WaitForSeconds(1);
        sequenceIndex = 0;
    }
}
