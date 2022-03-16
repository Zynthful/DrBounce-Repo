using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.AI;


public class TowardsTarget : BtNode
{
    private Transform m_movement;
    private float moveSpeed;
    private NavMeshAgent m_navMeshAgent;

    public TowardsTarget(NavMeshAgent navMeshAgent, float speed = 2)
    {
        moveSpeed = speed;
        m_navMeshAgent = navMeshAgent;
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

        if (blackboard.noBounceAIController == true)
        {
            m_navMeshAgent.destination = targetPosition;
            return NodeState.SUCCESS;
        }
        else
        {
            m_movement.position = Vector3.MoveTowards(m_movement.position, targetPosition, Time.deltaTime / moveSpeed);
        }

        //Debug.Log("Start Pos " + blackboard.startPosition + "  & target pos " + targetPosition);

        //throws up debug log if the target is zero, help from Chris 
        Vector3 target = (targetPosition - m_movement.position).normalized;
        if (!blackboard.HasTarget(true) && target.sqrMagnitude > 0)
        {
            m_movement.rotation = Quaternion.RotateTowards(m_movement.rotation, Quaternion.LookRotation(target), Time.deltaTime / .0045f);
        }

        blackboard.currentAction = Blackboard.Actions.PATROLING;

        return NodeState.SUCCESS;
    }

    public override string getName()
    {
        return "TowardsTargetNAV";
    }

}
