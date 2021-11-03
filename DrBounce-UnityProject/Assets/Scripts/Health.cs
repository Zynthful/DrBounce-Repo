using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using MoreMountains.Feedbacks;
using MoreMountains.Tools;
using UnityEngine.SceneManagement;


public class Health : MonoBehaviour
{
    private static int health = 100;
    private static int maxHealth = 100;
    private float minHealth = 0;

    private bool canSetStartingHealth = true;

    public delegate void CurrentHealth();
    public static event CurrentHealth ReportHealth;

    [Header("Game Events")]
    // Passes health percentage
    [SerializeField]
    private GameEventFloat onHealthChange = null;
    // Passes damage taken
    [SerializeField]
    private GameEventFloat onDamage = null;
    // Passes health healed
    [SerializeField]
    private GameEventFloat onHeal = null;
    [SerializeField]
    private GameEvent onDeath = null;

    [Header("Unity Events")]
    // Passes health percentage
    [SerializeField]
    private UnityEvent<float> _onHealthChange = null;
    [SerializeField]
    // Passes damage taken
    private UnityEvent<float> _onDamage = null;
    // Passes health healed
    [SerializeField]
    private UnityEvent<float> _onHeal = null;
    [SerializeField]
    private UnityEvent _onDeath = null;

    public MMProgressBar progressBar;

    private void Start()
    {
        health = maxHealth;

        onHealthChange?.Raise(GetHealthPercentage());
        _onHealthChange?.Invoke(GetHealthPercentage());
        progressBar.UpdateBar(health, minHealth, maxHealth);
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
        //health = 1;
        //progressBar.UpdateBar(health, minHealth, maxHealth);
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

        _onHealthChange?.Invoke(GetHealthPercentage());
        onHealthChange?.Raise(GetHealthPercentage());

        progressBar.UpdateBar(health, minHealth, maxHealth);
    }

    private void Damage(int amount) 
    {
        _onDamage?.Invoke(amount);
        onDamage?.Raise(amount);

        health -= amount;

        _onHealthChange?.Invoke(GetHealthPercentage());
        onHealthChange?.Raise(GetHealthPercentage());

        progressBar.UpdateBar(health, minHealth, maxHealth);

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


        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        Debug.Log("DIE (►__◄)");
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
    private float GetHealthPercentage()
    {
        return 100.0f * (health / maxHealth);
    }

    public bool ReturnDead()
    {
        return health <= 0;
    }
}
