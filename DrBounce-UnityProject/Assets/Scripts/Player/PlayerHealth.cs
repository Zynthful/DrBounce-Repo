using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerHealth : Health
{
    public delegate void PlayerDeath();
    public static event PlayerDeath OnPlayerDeath;

    [SerializeField]
    private UnityEvent onRespawn = null;

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
}
