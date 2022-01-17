using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsTargeting : BtNode
{
    public IsTargeting()
    {
    }

    public override NodeState evaluate(Blackboard blackboard)
    {
        if (blackboard.HasTarget())
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
