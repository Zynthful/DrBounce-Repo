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
        string path = Application.persistentDataPath + "/drsave.dat";
        FileStream stream = new FileStream(path, FileMode.Create);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static GameSaveData LoadGameData()
    {
        string path = Application.persistentDataPath + "/drsave.dat";
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
            //Debug.LogError("No existing save file found in path " + path);
            return null;

            /*
            Debug.LogWarning($"No existing save file found in path {path}. Creating new save...");
            return NewGameSave();
            */
        }
    }

    public static void DeleteLevelData()
    {
        if (File.Exists(Application.persistentDataPath + "/levelData.dat"))
        {
            File.Delete(Application.persistentDataPath + "/levelData.dat");
        }
        Checkpoint.ResetCurrentCheckpoint();
    }

    public static void DeleteGameData()
    {
        DeleteLevelData();
        if (File.Exists(Application.persistentDataPath + "/drsave.dat"))
        {
            File.Delete(Application.persistentDataPath + "/drsave.dat");
        }
    }

    public static bool LevelSaveExists(int levelIndex) 
    { 
        LevelSaveData data = LoadInLevel();
        if (data != null)
        {
            return data.level == levelIndex;
        }
        else
        {
            return false;
        }
    }

    public static GameSaveData NewGameSave()
    {
        GameSaveData save = new GameSaveData();
        SaveGameData(save);
        return save;
    }

    public static bool IsLevelUnlocked(int index)
    {
        if (index == 0)
            return true;

        GameSaveData data = SaveSystem.LoadGameData();
        if (data == null)
        {
            return false;
        }
        else return data.levelUnlocked >= index;
    }
}
