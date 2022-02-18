using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameSaveData
{
    public int levelUnlocked;

    public GameSaveData(int highestLevel)
    {
        levelUnlocked = highestLevel;
    }
}
