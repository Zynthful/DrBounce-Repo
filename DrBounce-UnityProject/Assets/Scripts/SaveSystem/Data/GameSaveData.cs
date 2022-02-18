using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSaveData
{
    public int levelUnlocked;

    public GameSaveData(int highestLevel)
    {
        levelUnlocked = highestLevel;
    }
}
