using UnityEngine;
using UnityEngine.SceneManagement;

public class WinState : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            EnteredEnd();
        }
    }

    /// <summary>
    /// Function that runs when the player beats the level
    /// </summary>
    private void EnteredEnd() 
    {
        //print("you win");

        //int sceneNumber = 100;
        //SceneManager.LoadScene(sceneNumber);
    }
}
