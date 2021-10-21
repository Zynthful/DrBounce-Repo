using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboTracker : MonoBehaviour
{
    // Number of consecutive bounces in one throw
    // Resets to 0 when: Catching, amountOfBounces resetting
    private int currentComboNum = 0;

    [Header("Events")]
    [SerializeField]
    private GameEventInt onComboIncrementWhilstActive = null;    // Occurs on bounce (excluding first bounce), Passes currentComboNum
    [SerializeField]
    private GameEventInt onComboIncrement = null;    // Occurs on bounce, Passes currentComboNum
    [SerializeField]
    private GameEvent onComboStart = null; // Occurs on first bounce
    [SerializeField]
    private GameEvent onComboEnd = null; // Occurs on currentCombo set to 0

    public void Increment()
    {
        SetComboNum(currentComboNum + 1);
    }

    public void SetComboNum(int value)
    {
        currentComboNum = value;

        if (currentComboNum >= 1)
        {
            onComboIncrement?.Raise(currentComboNum);

            if (currentComboNum == 1)
            {
                onComboStart?.Raise();
            }
        }
        // End combo if it is being reset to 0
        else if (currentComboNum <= 0)
        {
            onComboEnd?.Raise();
        }
        // Only called if the combo is already active
        else
        {
            onComboIncrementWhilstActive?.Raise(value);
        }
    }

    public int GetComboNum()
    {
        return (currentComboNum);
    }
}
