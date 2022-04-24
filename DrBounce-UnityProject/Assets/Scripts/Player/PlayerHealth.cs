using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerHealth : Health
{
    public delegate void PlayerDeath();
    public static event PlayerDeath OnPlayerDeath;

    public delegate void LowHealth();
    public static event LowHealth ShouldHeal;

    public UnityEvent onRespawn = null;
    public UnityEvent onLowHealthFirstTimeThisSave = null;
    public UnityEvent onHealedLowHealthFirstTimeThisSave = null;

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

    protected override void SetLowHealth(bool value)
    {
        base.SetLowHealth(value);

        // Update game save data
        GameSaveData data = SaveSystem.LoadGameData();
        if (data != null)
        {
            if (value && !data.beenOnLowHealth)
            {
                data.beenOnLowHealth = true;
                onLowHealthFirstTimeThisSave.Invoke();
                SaveSystem.SaveGameData(data);
            }
        }
    }

    protected override void DIE()
    {
        base.DIE();
    }

    public void Respawn()
    {
        onRespawn?.Invoke();
        ResetHealth();
        OnPlayerDeath?.Invoke();
    }

    public override void Damage(int amount, bool ignoreGod = false)
    {
        if (((float)health / (float)maxHealth) * 100.0f <= 50) 
        {
            ShouldHeal?.Invoke();
        }

        base.Damage(amount, ignoreGod);
    }

    public override void Heal(int amount)
    {
        bool wasOnLowHealth = isOnLowHealth;
        base.Heal(amount);

        // If we healed whilst on low health
        if (wasOnLowHealth)
        {
            // Update game save data
            GameSaveData data = SaveSystem.LoadGameData();
            if (data.beenOnLowHealth && !data.healedOnLowHealth)
            {
                data.healedOnLowHealth = true;
                onHealedLowHealthFirstTimeThisSave.Invoke();
                SaveSystem.SaveGameData(data);
            }
        }
    }
}