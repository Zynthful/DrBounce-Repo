using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetClose : BtNode {

    private string m_targetTag;
    private bool m_requireChild;

    public TargetClose(string targetTag, bool requireTagAsChild = false) 
    {
        this.m_targetTag = targetTag;
        this.m_requireChild = requireTagAsChild;
    }

    public override NodeState evaluate(Blackboard blackboard) {
        GameObject[] tagged = GameObject.FindGameObjectsWithTag(m_targetTag);

        if (m_requireChild)
        {
            List<GameObject> taggedChildren = new List<GameObject> { };

            foreach (GameObject t in tagged)
            {
                if (t.transform.parent = blackboard.owner.transform)
                {
                    taggedChildren.Add(t);
                }
            }

            tagged = taggedChildren.ToArray();
        }

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
        return "TargetClose";
    }
}
