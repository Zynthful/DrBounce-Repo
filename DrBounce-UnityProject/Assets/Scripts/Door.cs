using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [Tooltip("The door then opens and closes")]
    [SerializeField] private GameObject door = null;

    [Tooltip("All enemies below have to be dead for this door to open")]
    [SerializeField] private GameObject[] enemies;

    private bool open = false;
    bool isAnEnemyAlive = false;

    // Start is called before the first frame update
    void Start()
    {
        //door = GetComponentInChildren<GameObject>(); need to get first child not all children
    }

    private void Open() 
    {
        door.SetActive(false);
    }

    private void Close() 
    {
        door.SetActive(true);
    }

    private void CanOpen() 
    {
        isAnEnemyAlive = false;
        foreach (GameObject enemy in enemies) 
        {
            if (enemy.GetComponent<Enemy>().GetisDead() == false)
            {
                isAnEnemyAlive = true;
                Close();
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
        Enemy.OnDeath += CanOpen;
    }


    void OnDisable()
    {
        Enemy.OnDeath -= CanOpen;
    }
}
