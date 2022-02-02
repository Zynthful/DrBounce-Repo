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
        if (!blackboard.HasTarget(false))
        {
            return NodeState.FAILURE;
        }

        if (blackboard.startPosition == Vector3.zero)
        {
            blackboard.startPosition = blackboard.owner.transform.position;
            if (blackboard.startPosition == Vector3.zero)
            {
                return NodeState.FAILURE;
            }
        }

        Vector3 targetPosition = blackboard.target.spottedPosition;

        //Debug.Log("Start Pos " + blackboard.startPosition + "  & target pos " + targetPosition);

        m_movement.position = Vector3.MoveTowards(m_movement.position, targetPosition, Time.deltaTime / moveSpeed);

        if (!blackboard.HasTarget(true))
        {
            m_movement.rotation = Quaternion.RotateTowards(m_movement.rotation, Quaternion.LookRotation((targetPosition - m_movement.position).normalized), Time.deltaTime / .0045f);
        }

        blackboard.currentAction = Blackboard.Actions.PATROLING;

        return NodeState.SUCCESS;
    }

    public override string getName()
    {
        return "TowardsTargetNAV";
    }

}
