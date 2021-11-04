using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionDamageTrigger : MonoBehaviour
{
    [HideInInspector] public int damage;
    private void OnTriggerEnter(Collider other) 
    {
        EnemyHealth check = other.GetComponent<EnemyHealth>();
        if(check)
        {
            check.Damage(damage);
        }
    }
}
