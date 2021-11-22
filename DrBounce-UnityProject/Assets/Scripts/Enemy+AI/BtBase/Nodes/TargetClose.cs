using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetPlayer : BtNode {
    private string m_targetTag;

    public TargetPlayer(string targetTag) {
        this.m_targetTag = targetTag;
    }

    public override NodeState evaluate(Blackboard blackboard) {
        GameObject[] tagged = GameObject.FindGameObjectsWithTag(m_targetTag);

        GameObject closest = null;
        float closestDistance = float.MaxValue;

        foreach (GameObject gObject in tagged) {
            float distance = Vector3.Distance(blackboard.owner.transform.position, gObject.transform.position);
            if (distance < closestDistance) {
                closest = gObject;
                closestDistance = distance;
            }
        }

        if (closest != null)
        {
            blackboard.target = closest;
            return NodeState.SUCCESS;
        }
        return NodeState.FAILURE;
    }

    public override string getName()
    {
        return "TargetPlayer";
    }
}
