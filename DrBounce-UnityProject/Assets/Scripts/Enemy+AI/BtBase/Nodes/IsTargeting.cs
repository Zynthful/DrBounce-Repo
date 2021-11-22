using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsTargeting : BtNode
{
    private string m_targetTag;

    public IsTargeting(string targetTag)
    {
        m_targetTag = targetTag;
    }

    public override NodeState evaluate(Blackboard blackboard)
    {
        if (blackboard.target == null)
        {
            return NodeState.FAILURE;
        }

        if (blackboard.target.tag == m_targetTag && blackboard.target.activeInHierarchy)
        {
            return NodeState.SUCCESS;
        } else
        {
            return NodeState.FAILURE;
        }
    }

    public override string getName()
    {
        return "isTargeting";
    }

}
