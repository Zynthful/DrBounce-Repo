using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TargetInSight : BtNode
{

    private Blackboard m_blackboard;

    private float m_viewDist;
    private float m_sightAngle;
    public bool m_canSeePlayer;
    private NavMeshAgent m_navMeshAgent;

    /// <summary>
    /// Used to get variables from else where (custimisation)
    /// </summary>

    Transform enemyPosition;

    /// <summary>
    /// need to return true or false
    /// </summary>
    /// <param name="blackboard"></param>
    /// <returns></returns>
    /// 

    public TargetInSight(Blackboard blackboard, float viewDist, float sightAngle, NavMeshAgent navMeshAgent)
    {
        m_viewDist = viewDist;
        m_sightAngle = sightAngle;
        m_blackboard = blackboard;
        m_navMeshAgent = navMeshAgent;
    }


    public override NodeState evaluate(Blackboard blackboard)
    {
        // If player is in Line of Sight, set run blackboard.target.NewTarget(true, (player gameobject here)); to assign new target values to enemy AI
        enemyPosition = blackboard.owner.transform;

        Debug.Log("Successfuly reached " + getName());

        if (PlayerLosCheck())
        {
            m_blackboard.target.NewTarget(true, PlayerMovement.player.gameObject);
            enemyPosition.rotation = Quaternion.RotateTowards(enemyPosition.rotation, Quaternion.LookRotation((m_blackboard.target.playerObject.transform.position - enemyPosition.position).normalized), Time.deltaTime / .0045f);

            m_navMeshAgent.enabled = true;
            m_navMeshAgent.destination = m_blackboard.target.playerObject.transform.position;

            return NodeState.SUCCESS;
        }
        else
        {
            m_blackboard.target.NewTarget(false, null);
            return NodeState.FAILURE;
        }
    }

    /// <summary>
    /// needs to return name of node
    /// </summary>
    /// <returns></returns>
    /// 
    protected bool PlayerLosCheck()
    {
        if (Vector3.Dot(enemyPosition.TransformDirection(Vector3.forward), (PlayerMovement.player.position - enemyPosition.position).normalized) > (90 - m_sightAngle) / 90)
        {
            RaycastHit hit;

            Ray ray = new Ray(enemyPosition.position, (PlayerMovement.player.position - enemyPosition.position).normalized);

            if (Physics.Raycast(ray, out hit, m_viewDist) && hit.transform.root.CompareTag("Player"))
            {
                Debug.DrawLine(ray.origin, ray.origin + (PlayerMovement.player.position - enemyPosition.position).normalized * m_viewDist, Color.green);
                return true;
            }
            else
            {
                m_navMeshAgent.enabled = false;
                Debug.DrawLine(ray.origin, ray.origin + (PlayerMovement.player.position - enemyPosition.position).normalized * m_viewDist, Color.red);
            }
        }
        return false;
    }
    public override string getName()
    {
        return "TargetInSight";
    }
}
