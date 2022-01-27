using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class EnemyChase : BtNode
{

    private Blackboard m_blackboard;
    private NavMeshAgent m_navMeshAgent;
    private bool stopChasing = false;
    private NavMeshPath path;
    public EnemyChase(Blackboard blackboard, NavMeshAgent navMeshAgent)
    {
        m_blackboard = blackboard;
        m_navMeshAgent = navMeshAgent;
    }

    public override NodeState evaluate(Blackboard blackboard)
    {
        if (m_blackboard.spottedPlayer == true && stopChasing == false)
        {
            m_navMeshAgent.enabled = true;
            m_navMeshAgent.destination = PlayerMovement.player.transform.position;
        }

        if (m_blackboard.searchTime <= -10 || m_blackboard.noBounceAIController.navMeshAgent.path.status != NavMeshPathStatus.PathComplete)
        {
            //resets timer
            //disables navmesh if the enemy can't find the player and returns to its waypoints.

            stopChasing = true;
            m_blackboard.spottedPlayer = false;

            m_blackboard.noBounceAIController.navMeshAgent.destination = m_blackboard.noBounceAIController.patrolPoints[0];

            if (m_blackboard.noBounceAIController.patrolPoints[0].x >= m_blackboard.noBounceAIController.transform.position.x - 5 && m_blackboard.noBounceAIController.patrolPoints[0].x <= m_blackboard.noBounceAIController.transform.position.x + 5)
            {
                stopChasing = false;
                m_blackboard.noBounceAIController.navMeshAgent.enabled = false;
                m_blackboard.searchTime = 0;
            }

            //If enemy's x value is close to the waypoint location

            m_blackboard.noBounceAIController.canMove = true;
        }

        if(stopChasing == true)
        {
            path = new NavMeshPath();
            NavMesh.CalculatePath(m_blackboard.noBounceAIController.transform.position, PlayerMovement.player.transform.position, NavMesh.AllAreas, path);
            if (path.status == NavMeshPathStatus.PathComplete)
            {
                stopChasing = false;
                m_blackboard.searchTime = 0;
            }
        }

        if (getRange())
        {
            return NodeState.FAILURE;
        }

        else
        {
            return NodeState.FAILURE;
        }
    }

    public bool getRange()
    {
        if (Vector3.Distance(m_blackboard.noBounceAIController.transform.position, m_blackboard.noBounceAIController.navMeshAgent.destination) > 1 && m_blackboard.noBounceAIController.navMeshAgent.enabled == true)
        {
            Debug.Log("Outside of 1 unit");
            m_blackboard.searchTime -= Time.deltaTime;
            return false;
        }
        else
        {
            return true;
        }
    }

    public override string getName()
    {
        return "PlayerInRange";
    }
}
