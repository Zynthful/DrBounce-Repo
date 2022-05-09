using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FootstepAudio : MonoBehaviour
{
    [Header("Declarations")]
    [SerializeField]
    private PlayerMovement movement = null;

    [Header("Footstep Settings")]
    [Tooltip("Delay in seconds between each footstep whilst running at maximum speed.")]
    [SerializeField]
    private float minimumRunDelay = 0.6f;
    [Tooltip("The max speed by which the footstep delay has reached its minimum value.")]
    [SerializeField]
    private float maxSpeed = 20.0f;
    [Tooltip("Multiplier applied to the footstep delay for the first footstep when starting to move. Use this to make the first footstep on moving take more or less time to play than the rest.")]
    [SerializeField]
    private float initialDelayFactor = 0.35f;

    [Header("Events")]
    [SerializeField]
    private UnityEvent onFootstep = null;
    
    // Controls the run delay
    private bool startedDelay = false;
    // Controls initial delay
    private bool startedMoving = false;
    // Stores the active delay couroutine so that it can be stopped if needed
    private IEnumerator activeDelay = null;

    private void FixedUpdate()
    {
        if (movement.GetIsMoving() && movement.isGrounded && !movement.isSliding)
        {
            if (!startedDelay)
            {
                // Begin delay
                activeDelay = Delay(CalculateDelayFromSpeed(movement.trueVelocity.magnitude + 1));
                StartCoroutine(activeDelay);
            }
        }
        else
        {
            startedMoving = false;

            // Cancel our active delay if we're no longer valid to play a footstep
            if (startedDelay && activeDelay != null)
            {
                startedDelay = false;
                StopCoroutine(activeDelay);
                activeDelay = null;
            }
        }
    }

    /// <summary>
    /// Calculates the length of which to delay playing a footstep, using the given entity's speed.
    /// </summary>
    /// <param name="speed"></param>
    /// <returns></returns>
    private float CalculateDelayFromSpeed(float speed)
    {
        float speedFactor = minimumRunDelay * Mathf.Sqrt(maxSpeed / speed);   // Square root to create a curve of speed against delay, as opposed to linear

        // If this is our first time playing a footstep since we've started moving, apply the initial delay factor
        if (!startedMoving)
        {
            startedMoving = true;
            speedFactor *= initialDelayFactor;
        }
        return speedFactor;
    }

    /// <summary>
    /// Waits 'delay' seconds before handling the event, if we are still moving by then.
    /// </summary>
    /// <param name="delay">Seconds in which to wait before handling the event.</param>
    /// <returns></returns>
    private IEnumerator Delay(float delay)
    {
        startedDelay = true;
        yield return new WaitForSeconds(delay);
        startedDelay = false;

        // Check if we're still moving after the delay
        if (movement.GetIsMoving() && movement.isGrounded && !movement.isSliding)
        {
            onFootstep?.Invoke();
        }
    }
}