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
        if (blackboard.target == null || blackboard.target == Vector3.zero)
        {
            return NodeState.FAILURE;
        }

        if (blackboard.startPosition == Vector3.zero)
        {
            blackboard.startPosition = blackboard.owner.transform.localPosition;
            if (blackboard.startPosition == Vector3.zero)
            {
                return NodeState.FAILURE;
            }
        }

        Debug.Log("Start Pos " + blackboard.startPosition + "  & target pos " + blackboard.target);

        m_movement.localPosition = Vector3.MoveTowards(m_movement.localPosition, blackboard.target, Time.deltaTime / moveSpeed);
        m_movement.rotation = Quaternion.RotateTowards(m_movement.rotation, Quaternion.LookRotation((blackboard.target - m_movement.localPosition).normalized), Time.deltaTime / .0045f);
        return NodeState.SUCCESS;
    }

    public override string getName()
    {
        return "TowardsTargetNAV";
    }

}
