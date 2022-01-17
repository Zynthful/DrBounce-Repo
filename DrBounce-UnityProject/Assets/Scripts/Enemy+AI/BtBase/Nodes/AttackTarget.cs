using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackTarget : BtNode
{

    private Enemy.Target m_target;

    /// <summary>
    /// Used to get variables from else where (custimisation)
    /// </summary>
    public AttackTarget(Blackboard blackboard)
    {
        m_target = blackboard.target;
    }

    /// <summary>
    /// need to return true or false
    /// </summary>
    /// <param name="blackboard"></param>
    /// <returns></returns>
    public override NodeState evaluate(Blackboard blackboard)
    {
        // Stuff for shooting should be in Enemy.cs
        if (!blackboard.target.isPlayer)
        {
            return NodeState.FAILURE;
        }

        if (true)
        {
            return NodeState.SUCCESS;
        }
        else if (false)
        {
            return NodeState.FAILURE;
        }
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
