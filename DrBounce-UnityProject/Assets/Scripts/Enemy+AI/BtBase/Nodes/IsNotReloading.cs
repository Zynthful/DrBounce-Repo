using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsNotReloading : BtNode
{
    /// <summary>
    /// Used to get variables from else where (custimisation)
    /// </summary>
    /// 
    private Blackboard m_blackboard;
    public IsNotReloading(Blackboard blackboard)
    {
        m_blackboard = blackboard;
    }

    /// <summary>
    /// need to return true or false
    /// </summary>
    /// <param name="blackboard"></param>
    /// <returns></returns>
    public override NodeState evaluate(Blackboard blackboard)
    {
        if (m_blackboard.shotDelay >= 0 && m_blackboard.chargedShotDelay >= 0)
        {
            return NodeState.FAILURE;
        }

        else
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
        return "IsNotReloading";
    }
}
