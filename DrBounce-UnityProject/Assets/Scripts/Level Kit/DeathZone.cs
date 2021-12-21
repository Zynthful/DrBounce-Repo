using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathZone : MonoBehaviour
{
    public delegate void PlayerDeath();
    public static event PlayerDeath OnPlayerDeath;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Die();
        }
    }

    /// <summary>
    /// Kills the player
    /// </summary>
    private void Die()
    {
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        //reloads the current scene

        OnPlayerDeath?.Invoke();
    }
}
