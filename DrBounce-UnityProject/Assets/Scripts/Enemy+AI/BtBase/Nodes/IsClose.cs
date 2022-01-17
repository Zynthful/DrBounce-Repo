using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsClose : BtNode {
    private float m_distanceLimit = 10;

    public IsClose(float distanceLimit){
        m_distanceLimit = distanceLimit;
    }

    public override NodeState evaluate(Blackboard blackboard) {
        if (blackboard.target == null || (blackboard.target.playerObject == null && blackboard.target.spottedPosition == Vector3.zero)) {
            return NodeState.FAILURE;
        }

        float distance;
        if (blackboard.target.isPlayer)
        {
            distance = (blackboard.owner.transform.position - blackboard.target.playerObject.transform.position).magnitude;
        }
        else
        {
            distance = (blackboard.owner.transform.position - blackboard.target.spottedPosition).magnitude;
        }

        if (distance < m_distanceLimit) {
            return NodeState.SUCCESS;
        } else {
            return NodeState.FAILURE;
        }
    }

    public override string getName()
    {
        return "isClose";
    }

}
