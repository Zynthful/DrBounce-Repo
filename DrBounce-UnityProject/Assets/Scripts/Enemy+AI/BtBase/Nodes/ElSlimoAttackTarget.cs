using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class ElSlimoAttackTarget : BtNode
{
    protected Blackboard m_blackboard;

    protected ObjectPooler pool;

    protected BulletType m_bullet;

    protected BulletType m_chargedBullet;

    protected Transform targetPosition;
    protected Transform enemyPosition;
    protected float m_rateOfFire;

    protected float m_chargedShotDelay;

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

    public ElSlimoAttackTarget(Blackboard blackboard, float rateOfFire, float chargedShotDelay, BulletType bullet, BulletType chargedBullet, Vector3 originLos)
    {
        m_blackboard = blackboard;
        m_rateOfFire = rateOfFire;
        m_chargedBullet = chargedBullet;
        m_chargedShotDelay = chargedShotDelay;
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

        GameObject bullet;
        if(m_blackboard.chargedShotDelay <= 0)
            bullet = ShootCharged();
        else
            bullet = ShootNormal();

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
        m_blackboard.chargedShotDelay = m_chargedShotDelay;
        return pool.SpawnBulletFromPool("BossChargedShot", (enemyPosition.position + m_originLos) + (m_blackboard.aiController.transform.TransformDirection(Vector3.forward).normalized * 2.5f), Quaternion.Euler(m_blackboard.aiController.transform.TransformDirection(Vector3.forward)), (m_blackboard.target.playerObject.transform.position - (enemyPosition.position + m_originLos)).normalized, m_chargedBullet, null);
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
