using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetRandom : BtNode
{
    private string m_targetTag;

    public TargetRandom(string targetTag)
    {
        this.m_targetTag = targetTag;
    }

    public override NodeState evaluate(Blackboard blackboard)
    {
        GameObject[] tagged = GameObject.FindGameObjectsWithTag(m_targetTag);
        if (tagged.Length == 0) {
            return NodeState.FAILURE;
        }

        // pick a random one
        GameObject randomTag = tagged[Random.Range(0, tagged.Length)];
        blackboard.target = randomTag;
        m_nodeState = NodeState.SUCCESS;
        return m_nodeState;
    }

    public override string getName()
    {
        return "TargetRandom";
    }
}
