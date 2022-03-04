using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{

    public static void SaveInLevel(LevelSaveData data)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/levelData.dat";
        FileStream stream = new FileStream(path, FileMode.Create);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static LevelSaveData LoadInLevel()
    {
        string path = Application.persistentDataPath + "/levelData.dat";
        if(File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            LevelSaveData data = formatter.Deserialize(stream) as LevelSaveData;
            stream.Close();

            return data;
        }
        else
        {
            Debug.LogError("No existing save file found in path " + path);
            return null;
        }
    }

    public static void SaveGameData(GameSaveData data)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/save.dat";
        FileStream stream = new FileStream(path, FileMode.Create);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static GameSaveData LoadGameData()
    {
        string path = Application.persistentDataPath + "/save.dat";
        if(File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            GameSaveData data = formatter.Deserialize(stream) as GameSaveData;
            stream.Close();

            return data;
        }
        else
        {
            Debug.LogError("No existing save file found in path " + path);
            return null;
        }
    }

    public static bool LevelSaveExists(int levelIndex) { LevelSaveData data = LoadInLevel(); if (data.level == levelIndex) { return true; } else { return false; } }
}
