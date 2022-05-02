using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneManagement
{
    public static Scene[] GetLoadedScenes()
    {
        int countLoaded = SceneManager.sceneCount;
        Scene[] loadedScenes = new Scene[countLoaded];
        for (int i = 0; i < countLoaded; i++)
        {
            loadedScenes[i] = SceneManager.GetSceneAt(i);
        }
        return loadedScenes;
    }

    public static bool IsSceneLoaded(Scene scene)
    {
        Scene[] loadedScenes = GetLoadedScenes();
        for (int i = 0; i < loadedScenes.Length; i++)
        {
            if (loadedScenes[i] == scene)
            {
                return true;
            }
        }
        return false;

    }

    public static bool IsSceneLoaded(int sceneBuildIndex)
    {
        return IsSceneLoaded(SceneManager.GetSceneByBuildIndex(sceneBuildIndex));
    }

    public static bool IsSceneLoaded(string sceneName)
    {
        return IsSceneLoaded(SceneManager.GetSceneByName(sceneName));
    }
}