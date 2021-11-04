using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    /*
    public delegate void CurrentHealth();
    public static event CurrentHealth ReportHealth;
    */

    [Header("Health Settings")]
    protected int health = 100;

    [SerializeField]
    protected int maxHealth = 100;
    [SerializeField]
    private float deathDelay = 0.230f;

    protected bool canSetStartingHealth = true;

    [Header("Unity Events")]
    // Passes health value
    [SerializeField]
    protected UnityEvent<float> onHealthChange = null;
    // Passes health percentage normalized (between 0-1)
    [SerializeField]
    protected UnityEvent<float> onHealthChangeNormalized = null;
    // Passes damage taken
    [SerializeField]
    protected UnityEvent<float> onDamage = null;
    // Passes health healed
    [SerializeField]
    protected UnityEvent<float> onHeal = null;
    [SerializeField]
    protected UnityEvent onDeath = null;

    [Header("Game Events")]
    // Passes health percentage
    [SerializeField]
    private GameEventFloat _onHealthChange = null;
    // Passes health percentage normalized (between 0-1)
    [SerializeField]
    private GameEventFloat _onHealthChangeNormalized = null;
    // Passes damage taken
    [SerializeField]
    private GameEventFloat _onDamage = null;
    // Passes health healed
    [SerializeField]
    private GameEventFloat _onHeal = null;
    [SerializeField]
    private GameEvent _onDeath = null;

    protected virtual void Start()
    {
        ResetHealth();
    }

    protected virtual void Update()
    {
        if (canSetStartingHealth) UpdatedStartingHealth();
    }

    protected virtual void UpdatedStartingHealth()   //doesn't work in start : (
    {
        // Damage(30);
        canSetStartingHealth = false;
    }

    protected virtual void SetHealth(int value)
    {
        health = value;

        // Cap health
        if (health > maxHealth)
        {
            health = maxHealth;
        }

        else if (GetIsDead())
        {
            Invoke("DIE", deathDelay);
        }

        onHealthChange?.Invoke(health);
        onHealthChangeNormalized?.Invoke(GetHealthPercentageNormalized());
        _onHealthChange?.Raise(health);
        _onHealthChangeNormalized?.Raise(GetHealthPercentageNormalized());
    }

    public virtual void Heal(int amount) 
    {
        // print("i am healing: " + amount);

        onHeal?.Invoke(amount);
        _onHeal?.Raise(amount);

        SetHealth(health + amount);
    }

    public virtual void Damage(int amount) 
    {
        Debug.Log("dmg: " + amount);

        onDamage?.Invoke(amount);
        _onDamage?.Raise(amount);

        SetHealth(health - amount);
    }

    /*
    protected virtual void ReceiveRequest()
    {
        if(health < maxHealth)
        {
            ReportHealth?.Invoke();
        }
    }
    */

    protected virtual void DIE() 
    {
        onDeath?.Invoke();
        _onDeath?.Raise();

        Debug.Log($"DIE (►__◄) {gameObject}");
    }

    protected virtual void ResetHealth() 
    {
        SetHealth(maxHealth);
    }

    public int GetHealth() 
    {
        return health;
    }

    public bool GetIsAtFullHealth()
    {
        return (health >= maxHealth);
    }

    /// <summary>
    /// Returns health percentage as a float between 0-100
    /// </summary>
    /// <returns></returns>
    private float GetHealthPercentageNormalized()
    {
        return (float)health / (float)maxHealth;
    }

    public bool GetIsDead()
    {
        return health <= 0;
    }
}
