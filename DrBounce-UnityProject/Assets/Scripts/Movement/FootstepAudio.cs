using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootstepAudio : MonoBehaviour
{
    [Header("Declarations")]
    [SerializeField]
    private PlayerMovement movement = null;
    [Tooltip("Wwise Event to handle")]
    [SerializeField]
    private AkEvent footstepEvent = null;

    [Header("Footstep Settings")]
    [Tooltip("Delay in seconds between each footstep whilst running")]
    [SerializeField]
    private float runDelay = 0.6f;
    [Tooltip("Delay in seconds after the player starts movement before which the footstep can play")]
    [SerializeField]
    private float initialDelay = 0.3f;
    
    // Controls the run delay
    private bool startedDelay = false;
    // Controls the initial delay
    private bool startedMoving = false;

    private void FixedUpdate()
    {
        if (movement.GetIsMoving())
        {
            if (!startedDelay && movement.GetIsGrounded())
            {
                // Play initial delay if this is the first time we're delaying since we've started moving
                if (!startedMoving)
                {
                    startedMoving = true;
                    StartCoroutine(Delay(initialDelay));
                }
                else
                {
                    StartCoroutine(Delay(runDelay));
                }
            }
        }
        // Allow initial delay to play again, if we've stopped moving since
        else
        {
            startedMoving = false;
        }
    }

    /// <summary>
    /// Waits 'delay' seconds before handling the event, if we are still moving by then
    /// </summary>
    /// <param name="delay">Seconds in which to wait before handling the event</param>
    /// <returns></returns>
    private IEnumerator Delay(float delay)
    {
        startedDelay = true;
        yield return new WaitForSeconds(delay);
        startedDelay = false;

        // Check if we're still moving after the delay
        if (movement.GetIsMoving() && movement.GetIsGrounded())
        {
            footstepEvent?.HandleEvent(gameObject);
        }
    }
}
