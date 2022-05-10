using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Timer : MonoBehaviour
{
    private float timePaused = 0.0f;                            // Duration spent paused. Used for calculating checkpoint/finish time.
    private float startTime = 0.0f;                             // Current time when starting the timer

    private static float endTime = 0.0f;
    public static float GetEndTime() { return endTime; }

    [SerializeField]
    private List<float> checkpointTimes = new List<float>();    // List of times taken to get to each checkpoint
    private bool active = false;                                // Whether the timer is active or not

    [Header("Unity Events")]
    public UnityEvent onStartTimer = null;                      // Called every time a new timer is started (i.e., at the start and on every checkpoint)
    public UnityEvent<float> onCheckpointTime = null;           // Passes completion time from last checkpoint to the hit checkpoint
    public UnityEvent<float> onCompleteTime = null;             // Passes completion time for the level

    private void OnEnable()
    {
        CheckpointHit.onHit += Checkpoint;              // Listen for checkpoint hit
        LevelComplete.onComplete += OnLevelComplete;    // Listen for level complete
    }

    private void OnDisable()
    {
        CheckpointHit.onHit += Checkpoint;              // Stop listening for checkpoint hit
        LevelComplete.onComplete -= OnLevelComplete;    // Stop listening for level complete

    }

    private void Update()
    {
        // Update timePaused
        if (GameManager.s_Instance.paused && active)
        {
            timePaused += Time.unscaledDeltaTime;
        }
    }

    /// <summary>
    /// Adds new checkpoint time and starts a new timer.
    /// </summary>
    private void Checkpoint(CheckpointHit hit = null)
    {
        float checkpointTime = Time.time - startTime - timePaused;
        checkpointTimes.Add(checkpointTime);
        onCheckpointTime?.Invoke(checkpointTime);
        RestartTimer();
    }

    /// <summary>
    /// Starts a new timer.
    /// </summary>
    public void StartNewTimer()
    {
        active = true;
        checkpointTimes.Clear();
        RestartTimer();
        onStartTimer?.Invoke();
    }

    public void RestartTimer()
    {
        timePaused = 0.0f;
        startTime = Time.time;
    }

    private void OnLevelComplete()
    {
        endTime = Complete();
    }

    /// <summary>
    /// Level completed. Stops timer.
    /// </summary>
    public float Complete()
    {
        checkpointTimes.Add(Time.time - startTime - timePaused);
        active = false;
        endTime = LevelEndTime();
        onCompleteTime?.Invoke(endTime);
        return endTime;
    }

    /// <summary>
    /// Stops the active timer.
    /// </summary>
    /// <returns>The completion time.</returns>
    public float LevelEndTime()
    {
        float completionTime = 0.0f;
        for (int i = 0; i < checkpointTimes.Count; i++)
        {
            completionTime += checkpointTimes[i];
        }
        return completionTime;
    }
}