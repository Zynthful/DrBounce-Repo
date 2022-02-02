using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AttackTarget : BtNode
{
    private Blackboard m_blackboard;

    private ObjectPooler pool;

    private EnemyHealth health = null;
    private BulletType m_bullet;

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

    public AttackTarget(Blackboard blackboard, float rateOfFire, BulletType bullet)
    {
        m_blackboard = blackboard;
        m_rateOfFire = rateOfFire;
        m_bullet = bullet;
        pool = ObjectPooler.Instance;
    }

    public override NodeState evaluate(Blackboard blackboard)
    {
        // Stuff for shooting should be in Enemy.cs
        if (!m_blackboard.target.isPlayer)
        {
            return NodeState.FAILURE;
        }

        enemyPosition = m_blackboard.owner.transform;
        targetPosition = m_blackboard.target.playerObject.transform;
        GameObject bullet = Shoot();

        if (bullet != null)
        {
            m_blackboard.currentAction = Blackboard.Actions.ATTACKING;
            return NodeState.SUCCESS;
        }
        else
        {
            return NodeState.FAILURE;
        }
    }

    protected GameObject Shoot()
    {
        m_blackboard.shotDelay = m_rateOfFire;
        return pool.SpawnBulletFromPool("Bullet", enemyPosition.position, Quaternion.identity, (m_blackboard.target.playerObject.transform.position - enemyPosition.position).normalized, m_bullet, null);
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
