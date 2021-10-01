using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionDamageTrigger : MonoBehaviour
{
    public int damage;
    private void OnTriggerEnter(Collider other) 
    {
        Enemy check = other.GetComponent<Enemy>();
        if(check)
        {
            check.TakeDamage(damage);
        }
    }
}
