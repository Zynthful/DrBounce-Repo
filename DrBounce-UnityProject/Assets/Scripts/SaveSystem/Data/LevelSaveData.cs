using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSaveData
{
    public int checkpoint;
    public int health;
    public int level;
    public float[] position;
    public int charges;

    public LevelSaveData(int curLevel, int curCheckpoint, int curHealth, float[] curPosition, int curCharges)
    {
        level = curLevel;
        checkpoint = curCheckpoint;
        health = curHealth;
        position = curPosition;
        charges = curCharges;
    }
}
