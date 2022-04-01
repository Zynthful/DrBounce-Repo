using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;
using MoreMountains.Tools;

public class EnemyHealth : Health
{
    public delegate void Death();
    public event Death OnDeath;

    public GameEventFloat onBossDamage = null;

    [SerializeField]
    private Enemy enemy = null;

    protected override void SetHealth(int value, bool showBar)
    {
        base.SetHealth(value, showBar);
        switch (enemy.GetEnemyType())
        {
            case Enemy.EnemyType.Boss:
                onBossDamage?.Raise(value);
                break;
            case Enemy.EnemyType.Normal:
                break;
            default:
                break;
        }
    }

    protected override void DIE()
    {
        CombatManager.s_Instance.RemoveEnemy(GetComponent<Enemy>());

        base.DIE();

        OnDeath?.Invoke();

        Destroy(this);
    }
}
