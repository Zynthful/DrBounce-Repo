using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameSaveData
{
    public int levelUnlocked;                   // Index of the last level we've unlocked (e.g. if we've unlocked level 2, then this = 1)
    public bool beenOnLowHealth = false;        // Whether we've been on low health for the first time or not
    public bool healedOnLowHealth = false;      // Whether we've healed from low health for the first time or not

    public GameSaveData(int highestLevel = 0, bool _beenOnLowHealth = false, bool _healedOnLowHealth = false)
    {
        levelUnlocked = highestLevel;
        beenOnLowHealth = _beenOnLowHealth;
        healedOnLowHealth = _healedOnLowHealth;
    }
}
