using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Door : MonoBehaviour
{
    private bool open = false;
    private bool isAnEnemyAlive = false;

    [Header("Declarations")]
    /*
    [Tooltip("The door then opens and closes")]
    [SerializeField]
    private GameObject door = null;
    */
    [Tooltip("All enemies below have to be dead for this door to open")]
    [SerializeField]
    private GameObject[] enemies;

    [Header("Unity Events")]
    [SerializeField]
    private UnityEvent onOpen = null;
    [SerializeField]
    private UnityEvent onClose = null;

    // Start is called before the first frame update
    private void Start()
    {
        //door = GetComponentInChildren<GameObject>(); need to get first child not all children

        CheckIfCanOpen();
    }

    private void Open() 
    {
        // door.SetActive(false);

        onOpen?.Invoke();
    }

    private void Close() 
    {
        // door.SetActive(true);

        onClose?.Invoke();
    }

    private void CheckIfCanOpen() 
    {
        isAnEnemyAlive = false;
        foreach (GameObject enemy in enemies) 
        {        
            if (enemy != null)
            {
                if (!enemy.GetComponent<EnemyHealth>().GetIsDead())
                {
                    isAnEnemyAlive = true;
                    Close();
                }
            }
        }
        if (!isAnEnemyAlive) 
        {
            Open();
            isAnEnemyAlive = false;
        }
    }

    void OnEnable()
    {
        EnemyHealth.OnDeath += CheckIfCanOpen;
    }


    void OnDisable()
    {
        EnemyHealth.OnDeath -= CheckIfCanOpen;
    }
}
