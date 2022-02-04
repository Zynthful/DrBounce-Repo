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
        if (!blackboard.HasTarget(false)) {
            return NodeState.FAILURE;
        }

        if (blackboard.target.isPlayer)
        {
            m_agent.SetDestination(blackboard.target.playerObject.transform.position);
            blackboard.currentAction = Blackboard.Actions.CHASING;
        }
        else if (!blackboard.target.isPlayer)
        {
            m_agent.SetDestination(blackboard.target.spottedPosition);
            blackboard.currentAction = Blackboard.Actions.PATROLING;
        }

        return NodeState.SUCCESS;
    }

    public override string getName()
    {
        return "TowardsTargetNAV";
    }

}
