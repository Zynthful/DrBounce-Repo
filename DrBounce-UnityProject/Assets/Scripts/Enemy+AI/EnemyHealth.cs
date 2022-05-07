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

    private Coroutine attackedResetCor;

    [SerializeField]
    private Enemy enemy = null;

    protected override void Start()
    {
        base.Start();

        if(enemy == null)
            enemy = GetComponent<Enemy>();

        //Workaround fix to enemy colliders being disabled on prefab randomly
        if (GetComponent<Collider>())
        {
            if (GetComponent<Collider>().enabled == false)
            {
                GetComponent<Collider>().enabled = true;
            }

            if (GetComponent<Collider>().enabled == false)
            {
                Destroy(gameObject);
            }
        }
    }

    protected override void SetHealth(int value, bool showBar, bool ignoreGod = false)
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
        SetRecentAttacked();
    }

    public void SetRecentAttacked()
    {
        enemy.recentlyAttacked = true;
        if(attackedResetCor != null)
            StopCoroutine(attackedResetCor);
        attackedResetCor = StartCoroutine(RecentAttackReset());
    }

    protected override void DIE()
    {
        CombatManager.s_Instance.RemoveEnemy(GetComponent<Enemy>());

        base.DIE();

        OnDeath?.Invoke();

        Destroy(this);
    }

    IEnumerator RecentAttackReset()
    {
        yield return new WaitForSeconds(.4f);
        enemy.recentlyAttacked = false;
    }
}
