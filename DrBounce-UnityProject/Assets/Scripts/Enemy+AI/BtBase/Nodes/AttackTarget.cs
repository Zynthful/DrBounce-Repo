using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;
using System;

public class AttackTarget : BtNode
{
    private Blackboard m_blackboard;

    private Enemy.Target m_target;

    private ObjectPooler pool;

    private bool shootDelay;
    private Coroutine shootingDelayCoroutine;

    private EnemyAudio enemyAudio = null;
    [SerializeField]
    private EnemyHealth health = null;
    [SerializeField]
    private BulletType bullet;
    [SerializeField]
    private UnityEvent onShoot = null;

    Transform targetPosition;
    Transform enemyPosition;
    float m_rateOfFire;

    /// <summary>
    /// Used to get variables from else where (custimisation)
    /// </summary>

    /// <summary>
    /// need to return true or false
    /// </summary>
    /// <param name="blackboard"></param>
    /// <returns></returns>
    /// 

    public AttackTarget(Blackboard blackboard, float rateOfFire)
    {
        m_rateOfFire = rateOfFire;
    }
    public override NodeState evaluate(Blackboard blackboard)
    {
        Debug.Log("evaluating");
        // Stuff for shooting should be in Enemy.cs
        if (!blackboard.target.isPlayer)
        {
            return NodeState.FAILURE;
        }

        enemyPosition = blackboard.owner.transform;
        targetPosition = m_target.playerObject.transform;
        GameObject bullet = Shoot();

        if (bullet != null)
        {
            return NodeState.SUCCESS;
        }
        else
        {
            return NodeState.FAILURE;
        }
    }

    protected GameObject Shoot()
    {
        Debug.Log("Shooting;");
        m_blackboard.shotDelay = m_rateOfFire;
        onShoot?.Invoke();
        return pool.SpawnBulletFromPool("Bullet", enemyPosition.position, Quaternion.identity, (m_blackboard.target.playerObject.transform.position - enemyPosition.position).normalized, bullet, null);
    }

    public EnemyAudio GetAudio()
    {
        return enemyAudio;
    }

    /// <summary>
    /// needs to return name of node
    /// </summary>
    /// <returns></returns>
    public override string getName()
    {
        return "AttackTarget";
    }
}
