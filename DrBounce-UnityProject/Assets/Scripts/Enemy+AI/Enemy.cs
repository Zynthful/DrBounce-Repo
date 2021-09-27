using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;

public class Enemy : MonoBehaviour
{
    public enum EnemyTypes
    {
        BlueBack,
        YellowUp,
        RedForward,
    }

    public float health = 5f;

    public void TakeDamage(float amount)
    {
        HitFeedback?.PlayFeedbacks();
        health -= amount;
        if (health <= 0)
        {
            Die();
        }
    }
    public EnemyTypes eType;

    [Header("Feedbacks")]
    public MMFeedbacks HitFeedback;

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
        print("That's right baby! Our dog, " + this.name + ", is dead!");
        Destroy(gameObject);
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
