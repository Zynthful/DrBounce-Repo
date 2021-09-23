using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public enum EnemyTypes
    {
        BlueBack,
        YellowUp,
        RedForward,
    }

    public EnemyTypes eType;

    Enemy()
    {

    }

    ~Enemy()
    {
        Die();
    }

    protected GameObject Shoot()
    {
        return null;
    }

    public void Die()
    {
        Destroy(null);
    }

    private void Start()
    {
        switch (eType)
        {
            case EnemyTypes.BlueBack:
                GetComponent<MeshRenderer>().material.color = Color.blue;
                break;

            case EnemyTypes.YellowUp:
                GetComponent<MeshRenderer>().material.color = Color.yellow;
                break;

            case EnemyTypes.RedForward:
                GetComponent<MeshRenderer>().material.color = Color.red;
                break;
        }
    }
}
