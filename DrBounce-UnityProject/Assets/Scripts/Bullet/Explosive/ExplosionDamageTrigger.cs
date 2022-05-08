using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionDamageTrigger : MonoBehaviour
{
    public int damage;

    private enum HealthType{
        Enemy,
        Player,
    }

    [SerializeField] private HealthType[] damageTarget;

    private void OnTriggerEnter(Collider other) 
    {
        if(damageTarget.Length == 2)
        {
            EnemyHealth check = other.GetComponent<EnemyHealth>();
            if(check)
            {
                check.Damage(damage);
            }

            PlayerHealth check2 = other.GetComponent<PlayerHealth>();
            if(check2)
            {
                check2.Damage(damage);
            }
        }
        else if(damageTarget[0] == HealthType.Enemy)
        {
            EnemyHealth check = other.GetComponent<EnemyHealth>();
            if(check)
            {
                check.Damage(damage);
            }
        }
        else if(damageTarget[0] == HealthType.Player)
        {
            PlayerHealth check = other.GetComponent<PlayerHealth>();
            if(check)
            {
                check.Damage(damage);
            }
        }
    }
}
