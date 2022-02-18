using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadMidLevel : MonoBehaviour
{
    public void OnClickedLoad()
    {
        LevelSaveData data = SaveSystem.LoadInLevel();

        int[] checkpoint = Checkpoint.checkpointManagerInstance.GetCheckpointAndLevel();

        if(SceneManager.GetActiveScene().buildIndex != checkpoint[1])
        {
            
        }

        Transform player = PlayerMovement.player;

        Vector3 newPosition = new Vector3(data.position[0], data.position[1], data.position[2]);
        player.position = newPosition;

        LevelSaveData ll = new LevelSaveData(checkpoint[1], 
                                                checkpoint[0], 
                                                player.GetComponent<PlayerHealth>().GetHealth(), 
                                                new float[3]{player.position.x, player.position.y, player.position.z},
                                                player.GetComponentInChildren<Shooting>().GetCharges());
    }
}
