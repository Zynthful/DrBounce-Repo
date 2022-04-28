using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelSaveData
{
    public int checkpointID;
    public int health;
    public int level;
    public float[] position;
    public float[] rotation;
    public int[] unlocks;
    public int charges;

    public LevelSaveData(int curLevel, int curCheckpointID, int curHealth, float[] curPosition, float[] curRotation, int curCharges, int[] curSettings)
    {
        level = curLevel;
        checkpointID = curCheckpointID;
        health = curHealth;
        position = curPosition;
        rotation = curRotation;
        charges = curCharges;
        unlocks = curSettings;
    }
}
