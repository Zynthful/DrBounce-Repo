using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using MoreMountains.Tools;

public class Health : MonoBehaviour
{
    [Header("Health Settings")]
    protected int health = 100;

    [SerializeField]
    protected int maxHealth = 100;
    [SerializeField]
    private float deathDelay = 0.230f;

    [SerializeField]
    protected MMHealthBar healthBar;

    protected bool canSetStartingHealth = true;

    protected bool godmode = false;
    public virtual void SetGodmodeActive(bool value) { godmode = value; }

    [Header("Low Health Settings")]
    [SerializeField]
    [Tooltip("When this object's health drops below this percentage value, it will be considered as being on low health.")]
    [Range(0, 100.0f)]
    protected float lowHealthThreshold = 25.0f;

    protected bool isOnLowHealth = false;
    protected virtual void SetLowHealth(bool value)
    {
        if (value && !isOnLowHealth)
        {
            onLowHealth?.Invoke();
        }
        else if (!value && isOnLowHealth)
        {
            onNotLowHealth?.Invoke();
        }

        isOnLowHealth = value;
    }

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

    public delegate void NotLowHealth();
    public static event NotLowHealth HasHealed;

    public bool saveDamage;
    public int saveDamageValue;

    protected virtual void Start()
    {
        ResetHealth();

        if (saveDamage)
        {
            saveDamage = false;
            Damage(saveDamageValue, true);
        }
    }

    protected virtual void Update()
    {
        if (canSetStartingHealth) UpdatedStartingHealth();
    }

    protected virtual void UpdatedStartingHealth()   //doesn't work in start : (
    {
        canSetStartingHealth = false;
    }

    protected virtual void SetHealth(int value, bool showBar, bool ignoreGod = false)
    {
        if (godmode && !ignoreGod)
            return;

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

        SetLowHealth(((float)health / (float)maxHealth) * 100.0f <= lowHealthThreshold);

        onHealthChange?.Invoke(health);
        onHealthChangeNormalized?.Invoke(GetHealthPercentageNormalized());
        _onHealthChange?.Raise(health);
        _onHealthChangeNormalized?.Raise(GetHealthPercentageNormalized());
    }

    public virtual void Heal(int amount) 
    {
        if (GetIsDead())
            return;

        HasHealed?.Invoke();

        onHeal?.Invoke(amount);
        _onHeal?.Raise(amount);

        SetHealth(health + amount, true, true);
    }

    public virtual void Damage(int amount, bool ignoreGod = false) 
    {
        if (GetIsDead())
            return;

        onDamage?.Invoke(amount);
        _onDamage?.Raise(amount);

        SetHealth(health - amount, true, ignoreGod);

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
    public int GetMaxHealth() { return maxHealth; }

    public bool GetIsAtFullHealth() { return (health >= maxHealth); }

    /// <summary>
    /// Returns health percentage as a float between 0-100
    /// </summary>
    /// <returns></returns>
    private float GetHealthPercentageNormalized() { return (float)health / (float)maxHealth; }

    public bool GetIsDead() { return health <= 0; }
}
