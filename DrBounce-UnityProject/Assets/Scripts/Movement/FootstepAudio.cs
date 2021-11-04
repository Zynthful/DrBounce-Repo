using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootstepAudio : MonoBehaviour
{
    [Header("Declarations")]
    [SerializeField]
    private PlayerMovement movement = null;
    [Tooltip("Wwise Event to post")]
    [SerializeField]
    private AkEvent footstepEvent = null;

    [Header("Footstep Settings")]
    [Tooltip("Delay in seconds between each footstep whilst running")]
    [SerializeField]
    private float runDelay = 0.6f;
    [Tooltip("Delay in seconds after the player starts movement before which the footstep can play")]
    [SerializeField]
    private float initialDelay = 0.3f;

    private bool startedDelay = false;
    private bool startedMoving = false;

    private void FixedUpdate()
    {
        if (movement.GetIsMoving())
        {
            if (!startedDelay && movement.GetIsGrounded())
            {
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
        else
        {
            startedMoving = false;
        }
    }

    private IEnumerator Delay(float delay)
    {
        startedDelay = true;
        yield return new WaitForSeconds(delay);
        startedDelay = false;

        if (movement.GetIsMoving() && movement.GetIsGrounded())
        {
            footstepEvent?.HandleEvent(gameObject);
        }
    }
}
