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
        //print("open");
        collider.gameObject.SetActive(false);
    }

    private void Close() 
    {
        //print("closed");
        collider.gameObject.SetActive(true);
    }

    private void DelayForCheck() 
    {
        Invoke("CanOpen", 8);
    }

    private void CanOpen() 
    {
        isAnEnemyAlive = false;
        foreach (GameObject enemy in enemies) 
        {
            if (enemy != null)
            {
                isAnEnemyAlive = true;
                Close();
                //break;
                //return false;
            }
        }
        if (!isAnEnemyAlive) 
        {
            Open();
            isAnEnemyAlive = false;
        }
        //return true;
    }

    void OnEnable()
    {
        Enemy.OnDeath += DelayForCheck;
    }


    void OnDisable()
    {
        Enemy.OnDeath -= DelayForCheck;
    }
}
