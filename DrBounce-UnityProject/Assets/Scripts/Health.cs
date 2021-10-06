using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MoreMountains.Feedbacks;
using MoreMountains.Tools;
using UnityEngine.SceneManagement;


public class Health : MonoBehaviour
{
    private int health = 100;
    private int maxHealth = 100;

    public delegate void CurrentHealth();
    public static event CurrentHealth ReportHealth;

    [Header("Events")]
    // Passes health percentage
    [SerializeField]
    private GameEventFloat onHealthChange = null;
    // Passes damage taken
    [SerializeField]
    private GameEventFloat onDamage = null;
    // Passes health healed
    [SerializeField]
    private GameEventFloat onHeal = null;

    [Header("Feedbacks")]
    public MMFeedbacks DamageFeedback;
    public MMFeedbacks DeathFeedback;

    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
        onHealthChange?.Raise(100.0f * ((float)health / (float)maxHealth));
    }

    private void Update()
    {
        //print(health);
    }

    private void Heal(int amount) 
    {
        onHeal?.Raise(amount);

        health += amount;

        if (health > maxHealth) 
        {
            health = maxHealth;  
        }

        onHealthChange?.Raise(100.0f * ((float)health / (float)maxHealth));
    }

    private void Damage(int amount) 
    {
        DamageFeedback?.PlayFeedbacks();

        onDamage?.Raise(amount);

        health -= amount;

        onHealthChange?.Raise(100.0f * ((float) health / (float)maxHealth));

        if (health <= 0) 
        {
            Debug.Log("mortis");
            DeathFeedback?.PlayFeedbacks();
            Invoke("DIE",0.230f);
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
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        Debug.Log("DIE (►__◄)");
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
