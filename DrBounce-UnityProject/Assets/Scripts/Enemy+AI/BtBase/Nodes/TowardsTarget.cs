using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.AI;


public class TowardsTarget : BtNode
{
    private Transform m_movement;
    private float moveSpeed;

    public TowardsTarget(float speed = 2)
    {
        moveSpeed = speed;
    }

    public override NodeState evaluate(Blackboard blackboard)
    {
        if (m_movement == null)
        {
            m_movement = blackboard.owner.GetComponent<Transform>();
        }

        // if target is null, we can't move towards it!
        if (blackboard.target == null)
        {
            return NodeState.FAILURE;
        }

        if(blackboard.startPosition == Vector3.zero)
        {
            blackboard.startPosition = blackboard.owner.transform.position;
        }

        Debug.Log("Start Pos " + blackboard.startPosition + "  & target pos " + blackboard.target);
        m_movement.position = Vector3.Lerp(blackboard.startPosition, blackboard.target, Time.deltaTime / moveSpeed);
        if (Vector3.Distance(m_movement.position, blackboard.target) > 0.5)
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
