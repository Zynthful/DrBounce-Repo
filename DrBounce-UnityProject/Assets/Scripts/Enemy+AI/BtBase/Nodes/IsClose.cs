using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsClose : BtNode 
{

    private float m_distanceLimit = 10;
    private bool m_checkForPlayer;

    public IsClose(bool toPlayer, float distanceLimit)
    {
        m_checkForPlayer = toPlayer;
        m_distanceLimit = distanceLimit;
    }

    public override NodeState evaluate(Blackboard blackboard) {
        if (blackboard.target == null || (blackboard.target.playerObject == null && blackboard.target.spottedPosition == Vector3.zero)) {
            return NodeState.FAILURE;
        }

        float distance;
        if (m_checkForPlayer)
        {
            distance = (new Vector3(blackboard.owner.transform.position.x, blackboard.owner.transform.position.y / 2, blackboard.owner.transform.position.z) - new Vector3(blackboard.target.playerObject.transform.position.x, blackboard.target.playerObject.transform.position.y / 2, blackboard.target.playerObject.transform.position.z)).magnitude;
        }
        else
        {
            distance = (blackboard.owner.transform.position - blackboard.target.spottedPosition).magnitude;
        }

        if (distance < m_distanceLimit) 
        {
            return NodeState.SUCCESS;
        } 
        else 
        {
            return NodeState.FAILURE;
        }
    }

    public override string getName()
    {
        return "isClose";
    }

}
