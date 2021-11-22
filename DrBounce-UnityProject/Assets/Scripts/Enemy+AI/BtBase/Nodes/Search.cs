using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Search : BtNode
{
    private float m_distanceLimit = 10;

    public Search()
    {
        //m_distanceLimit = distanceLimit;
    }

    public override NodeState evaluate(Blackboard blackboard)
    {
        if (blackboard.target == null)
        {
            return NodeState.FAILURE;
        }

        float distance = (blackboard.owner.transform.position - blackboard.target.transform.position).magnitude;
        if (distance < m_distanceLimit)
        {
            return NodeState.SUCCESS;
        }
        else
        {
            return NodeState.FAILURE;
        }
    }

    public override string getName()
    {
        return "Search";
    }
}
