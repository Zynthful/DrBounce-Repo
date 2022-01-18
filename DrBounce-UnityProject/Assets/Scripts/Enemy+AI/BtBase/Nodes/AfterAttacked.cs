using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AfterAttacked : BtNode
{
    /// <summary>
    /// Used to get variables from else where (custimisation)
    /// </summary>
    public AfterAttacked()
    {
    }

    /// <summary>
    /// need to return true or false
    /// </summary>
    /// <param name="blackboard"></param>
    /// <returns></returns>
    public override NodeState evaluate(Blackboard blackboard)
    {
        // If the enemy has recently taken damage, this script should return NodeState.SUCCESS;

        if (true)
        {
            return NodeState.FAILURE;
        }
        else if (false)
        {
            return NodeState.SUCCESS;
        }
    }

    /// <summary>
    /// needs to return name of node
    /// </summary>
    /// <returns></returns>
    public override string getName()
    {
        return "AfterAttacked";
    }
}
