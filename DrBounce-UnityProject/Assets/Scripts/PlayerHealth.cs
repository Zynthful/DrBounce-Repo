using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : Health
{
    public delegate void PlayerDeath();
    public static event PlayerDeath OnPlayerDeath;

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
        OnPlayerDeath?.Invoke();
    }
}
