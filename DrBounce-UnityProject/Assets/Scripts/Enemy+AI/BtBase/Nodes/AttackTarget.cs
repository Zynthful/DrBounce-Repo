using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackTarget : BtNode
{

    private Vector3 m_target;

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
