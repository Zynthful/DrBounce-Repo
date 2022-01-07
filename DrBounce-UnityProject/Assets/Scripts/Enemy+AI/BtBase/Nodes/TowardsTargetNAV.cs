using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.AI;


public class TowardsTargetNAV : BtNode {
    private NavMeshAgent m_agent;

    public override NodeState evaluate(Blackboard blackboard) {
        if (m_agent == null) {
            m_agent = blackboard.owner.GetComponent<NavMeshAgent>();
        }

        // if target is null, we can't move towards it!
        if (blackboard.target == null) {
            return NodeState.FAILURE;
        }

        m_agent.SetDestination(blackboard.target);
        //Debug.Log("Agent: " + blackboard.owner.name + ", Target: " + blackboard.target.name);
        if ( Vector3.Distance(blackboard.owner.transform.position, blackboard.target) > 0.5 )
        {
            return NodeState.RUNNING;
        }

        return NodeState.SUCCESS;
    }

    public override string getName()
    {
        return "TowardsTargetNAV";
    }

}
