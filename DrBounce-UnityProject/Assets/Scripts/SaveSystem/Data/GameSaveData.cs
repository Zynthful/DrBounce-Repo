using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameSaveData
{
    public int levelUnlocked;
    public bool beenOnLowHealth = false;
    public bool healedOnLowHealth = false;

    public GameSaveData(int highestLevel = 0, bool _beenOnLowHealth = false, bool _healedOnLowHealth = false)
    {
        levelUnlocked = highestLevel;
        beenOnLowHealth = _beenOnLowHealth;
        healedOnLowHealth = _healedOnLowHealth;
    }
}
