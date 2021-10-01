using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    private int health = 1;

    private int maxHealth = 100;

    public delegate void CurrentHealth();
    public static event CurrentHealth ReportHealth;

    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Heal(int amount) 
    {
        health += amount;

        if (health > maxHealth) 
        {
            health = maxHealth;
            
        }
    }

    private void Damage(int amount) 
    {
        health -= amount;
        if (health <= 0) 
        {
            DIE();
        }
    }

    private void RecieveRequest()
    {
        if(health < maxHealth)
        {
            ReportHealth?.Invoke();
        }
    }

    private void DIE() 
    {
        Debug.Log("DIE (►__◄)");
    }

    private void OnEnable()
    {
        HealthPack.OnEntered += RecieveRequest;
        HealthPack.OnActivated += Heal;
    }

    private void OnDisable()
    {
        HealthPack.OnEntered -= RecieveRequest;
        HealthPack.OnActivated -= Heal;
    }
}
