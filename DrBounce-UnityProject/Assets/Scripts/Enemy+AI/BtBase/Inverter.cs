using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Behaviour tree implementation adapted from: https://hub.packtpub.com/building-your-own-basic-behavior-tree-tutorial/

public class Inverter : BtNode
{
    private BtNode m_node;
    
    public BtNode node
    {
        get { return m_node; }
    }

    public Inverter(BtNode node)
    {
        m_node = node;
    }

    public override NodeState evaluate(Blackboard blackboard)
    {
        switch( m_node.evaluate(blackboard) )
        {
            case NodeState.FAILURE:
                m_nodeState = NodeState.SUCCESS;
                return m_nodeState;
            case NodeState.SUCCESS:
                m_nodeState = NodeState.FAILURE;
                return m_nodeState;
            case NodeState.RUNNING:
                m_nodeState = NodeState.RUNNING;
                return m_nodeState;
        }

        m_nodeState = NodeState.SUCCESS;
        return m_nodeState;
    }

    public override string getName()
    {
        return "Invert";
    }

}
