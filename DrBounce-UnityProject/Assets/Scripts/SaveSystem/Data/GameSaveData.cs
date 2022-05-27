using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameSaveData
{
    public int levelUnlocked;                           // Index of the last level we've unlocked (e.g. if we've unlocked level 2, then this = 1)
    public bool beenOnLowHealth;                        // Whether we've been on low health for the first time or not
    public bool healedOnLowHealth;                      // Whether we've healed from low health for the first time or not
    public float[] levelPBTimes;                        // Level personal best times
    public float[] lastLevelTimes;                      // Level last completion time

    public GameSaveData(int highestLevel = 0, bool _beenOnLowHealth = false, bool _healedOnLowHealth = false, float[] _levelPBTimes = null, float[] _lastLevelTimes = null)
    {
        levelUnlocked = highestLevel;
        beenOnLowHealth = _beenOnLowHealth;
        healedOnLowHealth = _healedOnLowHealth;
        levelPBTimes = _levelPBTimes == null ? new float[3] : _levelPBTimes;            // insert number of levels here cos im bad plz tyty
        lastLevelTimes = _lastLevelTimes == null ? new float[3] : _lastLevelTimes;      // insert number of levels here cos im bad plz tyty
    }
}