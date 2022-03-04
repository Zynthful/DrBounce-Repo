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
            DontDestroyOnLoad(this);

            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

            Checkpoint.s_Instance.LoadLevelProgress(data.checkpoint);

            UnlockTracker.UnlockTypes[] unlocks = new UnlockTracker.UnlockTypes[data.unlocks.Length];
            for(int i = 0; i < data.unlocks.Length; i++)
            {
                unlocks[i] = (UnlockTracker.UnlockTypes)data.unlocks[i];
            }
            UnlockTracker.instance.NewUnlocks(unlocks);

            Transform player = PlayerMovement.player;

            Vector3 newPosition = new Vector3(data.position[0], data.position[1], data.position[2]);
            Quaternion rotation = new Quaternion(data.rotation[0], data.rotation[1], data.rotation[2], data.rotation[3]);
            player.position = newPosition;
            player.rotation = rotation;

            player.GetComponent<PlayerHealth>().Damage(player.GetComponent<PlayerHealth>().GetMaxHealth() - data.health);

            player.GetComponentInChildren<Shooting>().SetCharge(data.charges);

            Destroy(this);
        }
    }
}
