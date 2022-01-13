using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckBool : BtNode
{

    private bool m_bool;

    public CheckBool(ref bool check)
    {
        m_bool = check;
    }

    public override NodeState evaluate(Blackboard blackboard)
    {
        if (m_bool)
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
        return "checkBool";
    }
}
