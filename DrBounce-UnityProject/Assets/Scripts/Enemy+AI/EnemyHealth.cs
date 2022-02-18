using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;
using MoreMountains.Tools;

public class EnemyHealth : Health
{
    public delegate void Death();
    public static event Death OnDeath;

    protected override void DIE()
    {
        CombatAudioManager.s_Instance.RemoveEnemy(gameObject.GetInstanceID());

        base.DIE();

        OnDeath?.Invoke();

        Destroy(this);
    }
}
