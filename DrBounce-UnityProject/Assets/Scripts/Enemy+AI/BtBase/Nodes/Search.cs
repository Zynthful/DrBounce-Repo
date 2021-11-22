using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Search : BtNode
{
    public delegate NodeState ActionNodeDelegate(Blackboard blackboard);
    private ActionNodeDelegate m_action;

    public Search(ActionNodeDelegate action)
    {
        m_action = action;
        reset();
    }

    public override NodeState evaluate(Blackboard blackboard)
    {
        if (m_nodeState != NodeState.RUNNING)
        {
            return m_nodeState;
        }

        switch (m_action(blackboard))
        {
            case NodeState.SUCCESS:
                m_nodeState = NodeState.SUCCESS;
                return m_nodeState;
            case NodeState.RUNNING:
                m_nodeState = NodeState.RUNNING;
                return m_nodeState;
            case NodeState.FAILURE:
            default:
                m_nodeState = NodeState.FAILURE;
                return m_nodeState;
        }
    }

    public override string getName()
    {
        return "Action";
    }
}
