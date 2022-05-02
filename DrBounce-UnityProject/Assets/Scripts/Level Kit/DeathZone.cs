using UnityEngine;
using UnityEngine.Events;

public class DeathZone : MonoBehaviour
{
    public UnityEvent<GameObject, float> onObjectEnterWithSpeed = null;      // Passes GameObject enterring trigger and the speed of it at enterring
    public UnityEvent<GameObject> onObjectEnter = null;             // Passes GameObject enterring

    private void OnTriggerEnter(Collider other)
    {
        onObjectEnter?.Invoke(other.gameObject);

        PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            CharacterController controller = other.GetComponent<CharacterController>();
            if (controller != null)
            {
                onObjectEnterWithSpeed?.Invoke(other.gameObject, controller.velocity.magnitude);
            }

            // Kill player
            playerHealth.Damage(playerHealth.GetMaxHealth(), true);
        }
        else
        {
            onObjectEnterWithSpeed?.Invoke(other.gameObject, other.attachedRigidbody.velocity.magnitude);
        }
    }
}