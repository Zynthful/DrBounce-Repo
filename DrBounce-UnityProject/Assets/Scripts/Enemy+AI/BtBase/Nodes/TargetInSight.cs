using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetInSight : BtNode
{

    private Blackboard m_blackboard;

    /// <summary>
    /// Used to get variables from else where (custimisation)
    /// </summary>
    public TargetInSight(Blackboard board)
    {
        m_blackboard = board;
    }

    /// <summary>
    /// need to return true or false
    /// </summary>
    /// <param name="blackboard"></param>
    /// <returns></returns>
    public override NodeState evaluate(Blackboard blackboard)
    {
        // If player is in Line of Sight, set run blackboard.target.NewTarget(true, (player gameobject here)); to assign new target values to enemy AI

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
        return "TargetInSight";
    }
}
