using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveRemoveLevelData : MonoBehaviour
{
    public void DeleteLevelSaveData()
    {
        SaveSystem.DeleteLevelData();
    }
}
