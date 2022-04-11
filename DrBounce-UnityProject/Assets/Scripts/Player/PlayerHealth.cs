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

    [SerializeField]
    private UnityEvent onRespawn = null;

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

    public override void Damage(int amount)
    {
        if (((float)health / (float)maxHealth) * 100.0f <= 50) 
        {
            ShouldHeal?.Invoke();
        }

        base.Damage(amount);
    }
}
