using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    private int health = 1;

    private int maxHealth = 100;

    public delegate void CurrentHealth();
    public static event CurrentHealth ReportHealth;

    [SerializeField] private Slider slider = null;

    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
        UpdateUI();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Heal(int amount) 
    {
        health += amount;

        UpdateUI();

        if (health > maxHealth) 
        {
            health = maxHealth;
            
        }
    }

    private void Damage(int amount) 
    {
        health -= amount;

        UpdateUI();

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

    private void UpdateUI()
    {
        if(slider != null)
        {
            float healthPercent = ((float)health / (float)maxHealth) * 100;
            slider.value = healthPercent;
        }

    }

    private void OnEnable()
    {
        HealthPack.OnEntered += RecieveRequest;
        HealthPack.OnActivated += Heal;
        BulletMovement.OnHit += Damage;
    }

    private void OnDisable()
    {
        HealthPack.OnEntered -= RecieveRequest;
        HealthPack.OnActivated -= Heal;
        BulletMovement.OnHit -= Damage;
    }
}
