using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsNotReloading : BtNode
{
    /// <summary>
    /// Used to get variables from else where (custimisation)
    /// </summary>
    public IsNotReloading()
    {
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
        return "IsNotReloading";
    }
}
