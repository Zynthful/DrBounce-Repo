using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetClose : BtNode {

    private Vector3[] m_positions;

    private Blackboard m_blackboard;

    public TargetClose(Vector3[] points) 
    {
        this.m_positions = points;
    }

    public override NodeState evaluate(Blackboard blackboard) 
    {
        if (m_blackboard == null)
            m_blackboard = blackboard;

        Vector3 closest = blackboard.owner.transform.position;
        if (m_positions.Length > 0)
        {
            closest = m_positions[0];
            SetIndex(0);
        }
        float closestDistance = float.MaxValue;

        for (int i = 0; i < m_positions.Length; i++) {
            float distance = Vector3.Distance(blackboard.owner.transform.position, m_positions[i]);
            if (distance < closestDistance) {
                closest = m_positions[i];
                closestDistance = distance;
                SetIndex(i);
            }
        }

        if (closest != null)
        {
            blackboard.target.spottedPosition = closest;
            blackboard.startPosition = blackboard.owner.transform.position;
            return NodeState.SUCCESS;
        }

        //Debug.Log("Failed to target");
        return NodeState.FAILURE;
    }

    private void SetIndex(int value)
    {
        if (m_blackboard.noBounceAIController == null)
            m_blackboard.aiController.currentTargetIndex = value;
        else
            m_blackboard.noBounceAIController.currentTargetIndex = value;
    }

    public override string getName()
    {
        return "TargetClose";
    }
}
