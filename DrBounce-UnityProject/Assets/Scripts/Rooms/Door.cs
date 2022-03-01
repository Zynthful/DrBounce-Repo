using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Door : MonoBehaviour
{
    [Header("Declarations")]
    [SerializeField]
    [Tooltip("All enemies below have to be dead for this door to open.")]
    private EnemyHealth[] enemies = null;

    [Header("Unity Events")]
    public UnityEvent onOpen = null;
    public UnityEvent onClose = null;

    private void OnEnable()
    {
        // Listen to enemy death event for each enemy within our list
        for (int i = 0; i < enemies.Length; i++)
        {
            if (enemies[i] != null)
            {
                enemies[i].OnDeath += CheckIfCanOpen;
            }
        }

        CheckIfCanOpen();
    }

    private void OnDisable()
    {
        // Stop listening to enemy death events
        for (int i = 0; i < enemies.Length; i++)
        {
            enemies[i].OnDeath -= CheckIfCanOpen;
        }
    }

    private void Open() 
    {
        onOpen?.Invoke();
    }

    private void Close() 
    {
        onClose?.Invoke();
    }

    /// <summary>
    /// Checks if any enemies are alive. If not, the door opens, otherwise, it closes.
    /// </summary>
    private void CheckIfCanOpen() 
    {
        bool isAnEnemyAlive = false;

        foreach (EnemyHealth health in enemies) 
        {
            if (health != null)
            {
                if (!health.GetIsDead())
                {
                    isAnEnemyAlive = true;
                }
            }
        }

        if (!isAnEnemyAlive) 
        {
            Open();
        }
        else
        {
            Close();
        }
    }
}