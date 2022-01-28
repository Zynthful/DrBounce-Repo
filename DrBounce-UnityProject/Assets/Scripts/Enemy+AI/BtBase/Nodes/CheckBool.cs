using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckBool : BtNode
{

    private int m_integer;

    public CheckBool(int integer)
    {
        m_integer = integer;
    }

    public override NodeState evaluate(Blackboard blackboard)
    {
        switch (m_integer)
        {
            case 0:
                if (blackboard.aiController.canMove)
                {
                    return NodeState.SUCCESS;
                }
                else
                {
                    return NodeState.FAILURE;
                }

            case 1:
                if (blackboard.aiController.canAttack)
                {
                    return NodeState.SUCCESS;
                }
                else
                {
                    return NodeState.FAILURE;
                }

            case 3:
                if (blackboard.noBounceAIController.canMove)
                {
                    return NodeState.SUCCESS;
                }
                else
                {
                    return NodeState.FAILURE;
                }

            case 4:
                if (blackboard.noBounceAIController.canAttack)
                {
                    return NodeState.SUCCESS;
                }
                else
                {
                    return NodeState.FAILURE;
                }

            default:
                return NodeState.FAILURE;
        }
    }

    public override string getName()
    {
        return "checkBool";
    }
}
