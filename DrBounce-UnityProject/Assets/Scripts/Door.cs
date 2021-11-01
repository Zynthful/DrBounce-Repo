using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private bool open = false;
    private new BoxCollider collider = null;

    [SerializeField] private GameObject[] enemies;
    bool test = false;

    // Start is called before the first frame update
    void Start()
    {
        collider = GetComponentInChildren<BoxCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        //if (open) Open();
        //else Close();
    }

    private void Open() 
    {
        print("open");
        collider.gameObject.SetActive(false);
    }

    private void Close() 
    {
        print("closed");
        collider.gameObject.SetActive(true);
    }

    private void Test() 
    {
        //bool test = CanOpen();
        //if (test) Open();
        //else Close();

    }

    private void CanOpen() 
    {
        test = false;
        foreach (GameObject enemy in enemies) 
        {
            if (enemy.activeInHierarchy)
            {
                test = true;
                Close();
                //break;
                //return false;
            }
        }
        if (!test) 
        {
            Open();
            test = false;
        }
        //return true;
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
