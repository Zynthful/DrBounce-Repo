using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsTargeting : BtNode
{

    private bool m_playerCheck;

    public IsTargeting(bool player)
    {
        m_playerCheck = player;
    }

    public override NodeState evaluate(Blackboard blackboard)
    {
        if (blackboard.HasTarget(m_playerCheck))
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
        return "isTargeting";
    }

}
