using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private bool open = false;
    private new BoxCollider collider = null;

    [SerializeField] private GameObject[] enemies;
    bool isAnEnemyAlive = false;

    // Start is called before the first frame update
    void Start()
    {
        collider = GetComponentInChildren<BoxCollider>();
    }

    private void Open() 
    {
        collider.gameObject.SetActive(false);
    }

    private void Close() 
    {
        collider.gameObject.SetActive(true);
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
