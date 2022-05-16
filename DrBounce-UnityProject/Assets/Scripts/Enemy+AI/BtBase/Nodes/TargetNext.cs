using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetNext : BtNode
{
    private Vector3[] m_positions;

    public TargetNext(Vector3[] positions)
    {
        m_positions = positions;
    }

    public override NodeState evaluate(Blackboard blackboard)
    {
        if (m_positions.Length == 0)
        {
            //Debug.Log("Failed to target next");
            m_positions = new Vector3[0];
            m_positions[0] = blackboard.owner.transform.position;
        }

        if (blackboard.noBounceAIController == null)
        {
            blackboard.aiController.currentTargetIndex++;
            if (blackboard.aiController.currentTargetIndex > m_positions.Length - 1)
            {
                blackboard.aiController.currentTargetIndex = 0;
            }

            Vector3 nextPos = m_positions[blackboard.aiController.currentTargetIndex];

            blackboard.target.spottedPosition = nextPos;
            blackboard.startPosition = blackboard.owner.transform.position;

            m_nodeState = NodeState.SUCCESS;
            return m_nodeState;
        }

        else
        {
            blackboard.noBounceAIController.currentTargetIndex++;
            if (blackboard.noBounceAIController.currentTargetIndex > m_positions.Length - 1)
            {
                blackboard.noBounceAIController.currentTargetIndex = 0;
            }
            Vector3 nextPos = m_positions[blackboard.noBounceAIController.currentTargetIndex];

            blackboard.target.spottedPosition = nextPos;
            blackboard.startPosition = blackboard.owner.transform.position;

            m_nodeState = NodeState.SUCCESS;
            return m_nodeState;
        }

    }

    public override string getName()
    {
        return "TargetNext";
    }
}
