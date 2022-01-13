using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetClose : BtNode {

    private Vector3[] m_positions;

    public TargetClose(Vector3[] points) 
    {
        this.m_positions = points;
    }

    public override NodeState evaluate(Blackboard blackboard) 
    {
        Vector3 closest = m_positions[0];
        float closestDistance = float.MaxValue;

        foreach (Vector3 pos in m_positions) {
            float distance = Vector3.Distance(blackboard.owner.transform.position, pos);
            if (distance < closestDistance) {
                closest = pos;
                closestDistance = distance;
            }
        }

        if (closest != null)
        {
            blackboard.target = closest;
            blackboard.startPosition = blackboard.owner.transform.localPosition;
            return NodeState.SUCCESS;
        }

        Debug.Log("Failed to target");
        return NodeState.FAILURE;
    }

    public override string getName()
    {
        return "TargetClose";
    }
}
