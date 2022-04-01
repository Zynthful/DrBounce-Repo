using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;
using MoreMountains.Tools;

public class EnemyHealth : Health
{
    public delegate void Death();
    public event Death OnDeath;

    protected override void DIE()
    {
        CombatManager.s_Instance.RemoveEnemy(GetComponent<Enemy>());

        base.DIE();

        OnDeath?.Invoke();

        Destroy(this);
    }
}
