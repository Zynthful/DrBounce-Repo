using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttackTarget : BtNode
{
    float m_rateOfFire;
    int m_damage;
    float m_knockbackForce;

    public MeleeAttackTarget(float attackDelay, int damage, float knockbackForce)
    {
        m_rateOfFire = attackDelay;
        m_damage = damage;
        m_knockbackForce = knockbackForce;
    }

    public override NodeState evaluate(Blackboard blackboard)
    {
        if (!blackboard.target.isPlayer)
        {
            return NodeState.FAILURE;
        }

        blackboard.shotDelay = m_rateOfFire;
        blackboard.currentAction = Blackboard.Actions.ATTACKING;

        PlayerMovement.player.GetComponent<Health>().Damage(m_damage);
        PlayerMovement.instance.ApplyKnockback((blackboard.target.playerObject.transform.position - blackboard.owner.transform.position ).normalized, m_knockbackForce);

        return NodeState.SUCCESS;
    }

    /// <summary>
    /// needs to return name of node
    /// </summary>
    /// <returns></returns>
    public override string getName()
    {
        return "MeleeAttackTarget";
    }
}
