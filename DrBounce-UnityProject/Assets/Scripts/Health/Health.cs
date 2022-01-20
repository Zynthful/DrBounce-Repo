using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using MoreMountains.Tools;

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

    [SerializeField]
    protected MMHealthBar healthBar;

    protected bool canSetStartingHealth = true;

    [Header("Low Health Settings")]
    [SerializeField]
    [Tooltip("When this object's health drops below this percentage value, it will be considered as being on low health.")]
    [Range(0, 100.0f)]
    protected float lowHealthThreshold = 25.0f;

    protected bool isOnLowHealth = false;

    [Header("Unity Events")]
    // Passes health value
    [SerializeField]
    protected UnityEvent<float> onHealthChange = null;
    // Passes health percentage normalized (between 0-1)
    [SerializeField]
    protected UnityEvent<float> onHealthChangeNormalized = null;
    // Passes damage taken
    [SerializeField]
    protected UnityEvent<float> onDamage = null;    // Triggers when taking damage
    // Passes damage taken
    [SerializeField]
    protected UnityEvent<float> onInjured = null;   // Triggers when taking damage BUT NOT dying as a result of it
    // Passes health healed
    [SerializeField]
    protected UnityEvent<float> onHeal = null;
    [SerializeField]
    protected UnityEvent onDeath = null;
    [SerializeField]
    protected UnityEvent onLowHealth = null;
    [SerializeField]
    protected UnityEvent onNotLowHealth = null;

    [Header("Game Events")]
    // Passes health percentage
    [SerializeField]
    private GameEventFloat _onHealthChange = null;
    // Passes health percentage normalized (between 0-1)
    [SerializeField]
    private GameEventFloat _onHealthChangeNormalized = null;
    // Passes damage taken
    [SerializeField]
    private GameEventFloat _onDamage = null;    // Triggers when taking damage
    // Passes damage taken
    [SerializeField]
    private GameEventFloat _onInjured = null;   // Triggers when taking damage but not dying as a result of it
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
        canSetStartingHealth = false;
    }

    protected virtual void SetHealth(int value, bool showBar)
    {
        health = value;

        if (GetIsDead())
        {
            Invoke("DIE", deathDelay);
        }

        // Cap health
        else if (health > maxHealth)
        {
            health = maxHealth;
        }

        UpdateHealthBar(showBar);

        bool wasOnLowHealth = isOnLowHealth;    // Checked against to prevent calling low health events multiple times
        isOnLowHealth = ((float) health / (float) maxHealth) * 100.0f <= lowHealthThreshold;
        if (isOnLowHealth && !wasOnLowHealth)
        {
            onLowHealth?.Invoke();
        }
        else if (!isOnLowHealth && wasOnLowHealth)
        {
            onNotLowHealth?.Invoke();
        }

        onHealthChange?.Invoke(health);
        onHealthChangeNormalized?.Invoke(GetHealthPercentageNormalized());
        _onHealthChange?.Raise(health);
        _onHealthChangeNormalized?.Raise(GetHealthPercentageNormalized());
    }

    public virtual void Heal(int amount) 
    {
        onHeal?.Invoke(amount);
        _onHeal?.Raise(amount);

        SetHealth(health + amount, true);
    }

    public virtual void Damage(int amount) 
    {
        onDamage?.Invoke(amount);
        _onDamage?.Raise(amount);

        SetHealth(health - amount, true);

        // Only call injured events if we're not dead after taking damage
        if (!GetIsDead())
        {
            onInjured?.Invoke(amount);
            _onInjured?.Raise(amount);
        }
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

        // Debug.Log($"DIE (►__◄), {gameObject.name}");
    }

    protected virtual void ResetHealth() 
    {
        SetHealth(maxHealth, false);
    }

    protected virtual void UpdateHealthBar(bool showBar)
    {
        if (healthBar != null)
        {
            healthBar.UpdateBar(health, 0, maxHealth, showBar);
        }
    }

    public int GetHealth() { return health; }

    public bool GetIsAtFullHealth() { return (health >= maxHealth); }

    /// <summary>
    /// Returns health percentage as a float between 0-100
    /// </summary>
    /// <returns></returns>
    private float GetHealthPercentageNormalized() { return (float)health / (float)maxHealth; }

    public bool GetIsDead() { return health <= 0; }
}
