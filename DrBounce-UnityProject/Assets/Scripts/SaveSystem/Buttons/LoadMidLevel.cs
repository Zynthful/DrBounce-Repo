using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadMidLevel : MonoBehaviour
{
    public void OnClickedLoad()
    {
        LevelSaveData data = SaveSystem.LoadInLevel();

        if(SceneManager.GetActiveScene().buildIndex != data.level)
        {
            Debug.LogError("Load level failed, not on the correct level, this button should be disabled");
            return;
        }
        else
        {
            Transform player = PlayerMovement.player;

            Vector3 newPosition = new Vector3(data.position[0], data.position[1], data.position[2]);
            player.position = newPosition;

            Checkpoint.checkpointManagerInstance.LoadLevelProgress(data.checkpoint);

            player.GetComponent<PlayerHealth>().Damage(player.GetComponent<PlayerHealth>().GetMaxHealth() - data.health);

            player.GetComponentInChildren<Shooting>().SetCharge(data.charges);
        }
    }
}
