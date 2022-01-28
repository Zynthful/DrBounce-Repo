using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckIfStunned : BtNode
{
    Stun m_stun;
    /// <summary>
    /// Used to get variables from else where (custimisation)
    /// </summary>
    public CheckIfStunned(Stun stun)
    {
        m_stun = stun;
    }

    /// <summary>
    /// need to return true or false
    /// </summary>
    /// <param name="blackboard"></param>
    /// <returns></returns>
    public override NodeState evaluate(Blackboard blackboard)
    {
        if (m_stun.IsStunned())
        {
            return NodeState.SUCCESS;   //is stunned, can't move or shot
        }
        else
        {
            return NodeState.FAILURE;   //not stunned, can move or shot
        }
    }

    /// <summary>
    /// needs to return name of node
    /// </summary>
    /// <returns></returns>
    public override string getName()
    {
        return "CheckIfStunned";
    }

}
