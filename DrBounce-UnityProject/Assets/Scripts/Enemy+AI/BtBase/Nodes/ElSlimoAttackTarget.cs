using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class ElSlimoAttackTarget : BtNode
{
    protected Blackboard m_blackboard;

    protected ObjectPooler pool;

    protected BulletType m_bullet;

    protected Transform targetPosition;
    protected Transform enemyPosition;
    protected float m_rateOfFire;

    protected Vector3 m_originLos;

    /// <summary>
    /// Used to get variables from else where (custimisation)
    /// </summary>

    /// <summary>
    /// need to return true or false
    /// </summary>
    /// <param name="blackboard"></param>
    /// <returns></returns>
    /// 

    public ElSlimoAttackTarget(Blackboard blackboard, float rateOfFire, BulletType bullet, Vector3 originLos)
    {
        m_blackboard = blackboard;
        m_rateOfFire = rateOfFire;
        m_bullet = bullet;
        pool = ObjectPooler.Instance;
        m_originLos = originLos;
    }

    public override NodeState evaluate(Blackboard blackboard)
    {
        if (!m_blackboard.target.isPlayer)
        {
            return NodeState.FAILURE;
        }

        enemyPosition = m_blackboard.owner.transform;
        targetPosition = m_blackboard.target.playerObject.transform;
        GameObject bullet = ShootNormal();

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

    protected GameObject ShootNormal()
    {
        m_blackboard.shotDelay = m_rateOfFire;
        return pool.SpawnBulletFromPool("Bullet", enemyPosition.position + m_originLos, Quaternion.identity, (m_blackboard.target.playerObject.transform.position - (enemyPosition.position + m_originLos)).normalized, m_bullet, null);
    }

    protected GameObject ShootCharged()
    {
        m_blackboard.shotDelay = m_rateOfFire;
        return pool.SpawnBulletFromPool("Bullet", enemyPosition.position + m_originLos, Quaternion.identity, (m_blackboard.target.playerObject.transform.position - (enemyPosition.position + m_originLos)).normalized, m_bullet, null);
    }

    /// <summary>
    /// needs to return name of node
    /// </summary>
    /// <returns></returns>
    public override string getName()
    {
        return "ElSlimoAttackTarget";
    }

}
