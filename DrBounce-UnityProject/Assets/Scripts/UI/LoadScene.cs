using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    public void LoadSceneByIndex(int index)
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(index);
    }

    public void LoadSceneByName(string name)
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(name);
    }
}
