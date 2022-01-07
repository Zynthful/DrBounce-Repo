using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetRandom : BtNode
{
    private string m_targetTag;
    private bool m_requireChild;

    public TargetRandom(string targetTag, bool requireTagAsChild = false)
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

        if (tagged.Length == 0) {
            return NodeState.FAILURE;
        }

        // pick a random one
        GameObject randomTag = tagged[Random.Range(0, tagged.Length)];
        blackboard.target = randomTag.transform.position;
        blackboard.startPosition = blackboard.owner.transform.position;
        m_nodeState = NodeState.SUCCESS;
        return m_nodeState;
    }

    public override string getName()
    {
        return "TargetRandom";
    }
}
