using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TargetNext : BtNode
{
    private Vector3[] m_positions;

    public TargetNext(Vector3[] positions)
    {
        this.m_positions = positions;
    }

    public override NodeState evaluate(Blackboard blackboard)
    {
        if (m_positions.Length == 0)
        {
            Debug.Log("Failed to target next");
            return NodeState.FAILURE;
        }

        if (blackboard.aiController != null)
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
        // pick the next one

    }

    public override string getName()
    {
        return "TargetNext";
    }
}
