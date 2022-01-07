using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetNext : BtNode
{
    private string m_targetTag;
    private bool m_requireChild;

    public TargetNext(string targetTag, bool requireTagAsChild = false)
    {
        this.m_targetTag = targetTag;
        this.m_requireChild = requireTagAsChild;
    }

    public override NodeState evaluate(Blackboard blackboard)
    {
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
