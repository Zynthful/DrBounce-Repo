using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetNext : BtNode
{
    private string m_targetTag;

    public TargetNext(string targetTag)
    {
        this.m_targetTag = targetTag;
    }

    public override NodeState evaluate(Blackboard blackboard)
    {
        GameObject[] tagged = GameObject.FindGameObjectsWithTag(m_targetTag);
        if (tagged.Length == 0)
        {
            return NodeState.FAILURE;
        }

        // pick the next one
        blackboard.aiController.currentTargetIndex++;
        if(blackboard.aiController.currentTargetIndex > tagged.Length - 1)
        {
            blackboard.aiController.currentTargetIndex = 0;
        }
        GameObject nextTag = tagged[blackboard.aiController.currentTargetIndex];
        blackboard.target = nextTag;
        m_nodeState = NodeState.SUCCESS;
        return m_nodeState;
    }

    public override string getName()
    {
        return "TargetNext";
    }
}
