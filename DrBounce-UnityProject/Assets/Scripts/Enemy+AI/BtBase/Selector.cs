using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selector : BtNode
{
    protected List<BtNode> m_nodes;
    protected int runningNode;

    public Selector(List<BtNode> nodes)
    {
        m_nodes = nodes;
        reset();
    }

    public Selector(params BtNode[] nodes)
    {
        m_nodes = new List<BtNode>();
        foreach (BtNode node in nodes)
        {
            m_nodes.Add(node);
        }
        reset();
    }

    public override IEnumerable<BtNode> children()
    {
        return m_nodes;
    }

    public override void reset()
    {
        m_nodeState = NodeState.RUNNING;
        runningNode = 0;

        foreach (BtNode node in m_nodes)
        {
            node.reset();
        }
    }

    public override NodeState evaluate(Blackboard board) 
    {
        return evaluateStep(board);
    }

    public NodeState evaluateStep(Blackboard blackboard)
    {
        foreach (BtNode node in m_nodes)
        {
            switch (node.evaluate(blackboard))
            {
                case NodeState.SUCCESS:
                    m_nodeState = NodeState.SUCCESS;
                    return m_nodeState;
                case NodeState.FAILURE:
                    continue;
                case NodeState.RUNNING:
                    m_nodeState = NodeState.RUNNING;
                    return m_nodeState;
                default:
                    continue;
            }
        }

        m_nodeState = NodeState.FAILURE;
        return m_nodeState;
    }

    public override string getName()
    {
        return "Selector";
    }

}
