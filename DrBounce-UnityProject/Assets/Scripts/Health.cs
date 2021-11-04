using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;


public class Health : MonoBehaviour
{
    private static int health = 100;
    private static int maxHealth = 100;

    private bool canSetStartingHealth = true;

    public delegate void CurrentHealth();
    public static event CurrentHealth ReportHealth;

    public delegate void PlayerDeath();
    public static event PlayerDeath OnPlayerDeath;

    [Header("Events")]
    // Passes health percentage
    [SerializeField]
    private GameEventFloat onHealthChange = null;
    // Passes health percentage normalized (between 0-1)
    [SerializeField]
    private GameEventFloat onHealthChangeNormalized = null;
    // Passes damage taken
    [SerializeField]
    private GameEventFloat onDamage = null;
    // Passes health healed
    [SerializeField]
    private GameEventFloat onHeal = null;
    [SerializeField]
    private GameEvent onDeath = null;

    [Header("Unity Events")]
    // Passes health value
    [SerializeField]
    private UnityEvent<float> _onHealthChange = null;
    // Passes health percentage normalized (between 0-1)
    [SerializeField]
    private UnityEvent<float> _onHealthChangeNormalized = null;
    // Passes damage taken
    [SerializeField]
    private UnityEvent<float> _onDamage = null;
    // Passes health healed
    [SerializeField]
    private UnityEvent<float> _onHeal = null;
    [SerializeField]
    private UnityEvent _onDeath = null;

    private void Start()
    {
        ResetHealth();
    }

    private void Update()
    {
        //print(health);
        if (canSetStartingHealth) UpdatedStartingHealth();
    }

    private void OnEnable()
    {
        HealthPack.OnActivated += Heal;
        Shooting.OnActivated += Heal;
        BulletMovement.OnHit += Damage;
    }

    private void OnDisable()
    {
        HealthPack.OnActivated -= Heal;
        Shooting.OnActivated -= Heal;
        BulletMovement.OnHit -= Damage;
    }

    private void UpdatedStartingHealth()   //doesn't work in start : (
    {
        Damage(30);
        canSetStartingHealth = false;
    }

    private void Heal(int amount) 
    {
        //print("i am healing: " + amount);

        _onHeal?.Invoke(amount);
        onHeal?.Raise(amount);

        health += amount;

        if (health > maxHealth) 
        {
            health = maxHealth;  
        }

        InvokeHealthChange();
    }

    private void Damage(int amount) 
    {
        _onDamage?.Invoke(amount);
        onDamage?.Raise(amount);

        health -= amount;

        InvokeHealthChange();

        if (health <= 0) 
        {
            //Debug.Log("mortis");

            onDeath?.Raise();
            _onDeath?.Invoke();

            Invoke("DIE", 0.230f);
        }
    }

    private void ReceiveRequest()
    {
        if(health < maxHealth)
        {
            ReportHealth?.Invoke();
        }
    }

    private void DIE() 
    {
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        OnPlayerDeath?.Invoke();

        Debug.Log("DIE (►__◄)");

        ResetHealth();
    }

    /// <summary>
    /// Calls onHealthChange events, which currently updates the ui baby
    /// </summary>
    private void InvokeHealthChange() 
    {
        _onHealthChange?.Invoke(health);
        _onHealthChangeNormalized?.Invoke(GetHealthPercentageNormalized());
        onHealthChange?.Raise(health);
        onHealthChangeNormalized?.Raise(GetHealthPercentageNormalized());
    }

    private void ResetHealth() 
    {
        health = maxHealth;

        InvokeHealthChange();
    }

    public static int ReturnHealth() 
    {
        return health;
    }

    public static bool ReturnHealthNotMax()
    {
        return (health < maxHealth);
    }

    /// <summary>
    /// Returns health percentage as a float between 0-100
    /// </summary>
    /// <returns></returns>
    private float GetHealthPercentageNormalized()
    {
        return (float)health / (float)maxHealth;
    }

    public bool ReturnDead()
    {
        return health <= 0;
    }
}
