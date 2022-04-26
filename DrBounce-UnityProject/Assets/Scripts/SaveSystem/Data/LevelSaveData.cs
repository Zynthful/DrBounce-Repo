using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelSaveData
{
    public int checkpoint;
    public int health;
    public int level;
    public float[] position;
    public float[] rotation;
    public int[] unlocks;
    public int charges;

    public LevelSaveData(int curLevel, int curCheckpoint, int curHealth, float[] curPosition, float[] curRotation, int curCharges, int[] curSettings)
    {
        level = curLevel;
        checkpoint = curCheckpoint;
        health = curHealth;
        position = curPosition;
        rotation = curRotation;
        charges = curCharges;
        unlocks = curSettings;
    }
}
