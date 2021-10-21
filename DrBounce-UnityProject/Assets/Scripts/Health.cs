using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MoreMountains.Feedbacks;
using MoreMountains.Tools;
using UnityEngine.SceneManagement;


public class Health : MonoBehaviour
{
    private static int health = 100;
    private static int maxHealth = 100;
    private float minHealth = 0;

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

    public MMProgressBar progressBar;

    bool canSetStartingHealth = true;


    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;

        onHealthChange?.Raise(100.0f * ((float)health / (float)maxHealth));
        progressBar.UpdateBar(health, minHealth, maxHealth);
    }

    private void Update()
    {
        //print(health);
        if (canSetStartingHealth) UpdatedStartingHealth();
    }

    private void UpdatedStartingHealth()   //doesn't work in start : (
    {
        //health = 1;
        //progressBar.UpdateBar(health, minHealth, maxHealth);
        //Damage(0);
        canSetStartingHealth = false;
    }

    private void Heal(int amount) 
    {
        //print("i am healing: " + amount);

        onHeal?.Raise(amount);


        health += amount;

        progressBar.UpdateBar(health, minHealth, maxHealth);

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

        progressBar.UpdateBar(health, minHealth, maxHealth);

        onHealthChange?.Raise(100.0f * ((float) health / (float)maxHealth));

        if (health <= 0) 
        {
            //Debug.Log("mortis");
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
        HealthPack.OnActivated += Heal;
        BulletMovement.OnHit += Damage;
    }

    private void OnDisable()
    {
        HealthPack.OnActivated -= Heal;
        BulletMovement.OnHit -= Damage;
    }

    public static int ReturnHealth() 
    {
        return health;
    }

    public static bool ReturnHealthNotMax()
    {
        return (health < maxHealth);

    }

    public bool ReturnDead()
    {
        return health <= 0;
    }
}
