using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadMidLevel : MonoBehaviour
{

    private void Awake()
    {
        Debug.Log("Before " + GetComponent<Button>().interactable);
        if (SaveSystem.LevelSaveExists(SceneManager.GetActiveScene().buildIndex))
            GetComponent<Button>().interactable = true;
        else
            GetComponent<Button>().interactable = false;
        Debug.Log("After " + GetComponent<Button>().interactable);
        Checkpoint.s_Instance.OnCheckpointReached.AddListener(ActivateLoadButton);
    }

    void ActivateLoadButton()
    {
        Debug.Log("Checkpoint hit! Updating interactable status");
        GetComponent<Button>().interactable = true;
        Debug.Log("Interactable? " + GetComponent<Button>().interactable);
    }

    public void OnClickedLoad()
    {
        LevelSaveData data = SaveSystem.LoadInLevel();

        if(SceneManager.GetActiveScene().buildIndex != data.level)
        {
            Debug.LogError("Load level failed, not on the correct level, this button should be disabled");
            Debug.LogError("Current Level " + SceneManager.GetActiveScene().buildIndex + ", desired level: " + data.level);
            return;
        }
        else
        {
            Checkpoint.s_Instance.ReloadFromSaveProgress();
        }
    }
}
