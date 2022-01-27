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
        // If the player has been spotted, and the stopchasing hasn't been activated by the timer running out or the player being out of reach
        if (m_blackboard.spottedPlayer == true && stopChasing == false)
        {
            //re-enable Navmesh and target the player
            m_navMeshAgent.enabled = true;
            m_navMeshAgent.destination = PlayerMovement.player.transform.position;
        }

        if (m_blackboard.searchTime <= -10 || m_blackboard.noBounceAIController.navMeshAgent.path.status != NavMeshPathStatus.PathComplete)
        {
            //resets timer
            //disables navmesh if the enemy can't find the player and returns to its waypoints.

            stopChasing = true;
            m_blackboard.spottedPlayer = false;

            //Set the navmesh destination to the first patrol point in the list
            m_blackboard.noBounceAIController.navMeshAgent.destination = m_blackboard.noBounceAIController.patrolPoints[0];

            //Once the patrol point has been reached
            if (m_blackboard.noBounceAIController.patrolPoints[0].x >= m_blackboard.noBounceAIController.transform.position.x - 5 && m_blackboard.noBounceAIController.patrolPoints[0].x <= m_blackboard.noBounceAIController.transform.position.x + 5)
            {
                //Disable the timer, navmesh, set stopchasing to false, allowing the enemy to target the player again if spotted
                stopChasing = false;
                m_blackboard.noBounceAIController.navMeshAgent.enabled = false;
                m_blackboard.searchTime = 0;
            }

            //If enemy's x value is close to the waypoint location

            m_blackboard.noBounceAIController.canMove = true;
        }

        //This section allows the enemy to re-target the player if they're seen while travelling back to a waypoint

        //If the chase timer has run out or the player has gone out of reach, but if the player can still be seen
        if (stopChasing == true && m_blackboard.spottedPlayer == true)
        {
            //Test if the player can be reached by the enemy
            path = new NavMeshPath();
            NavMesh.CalculatePath(m_blackboard.noBounceAIController.transform.position, PlayerMovement.player.transform.position, NavMesh.AllAreas, path);

            //If they can:
            if (path.status == NavMeshPathStatus.PathComplete)
            {
                //Set stop chasing to false, allowing the player to be chased again if spotted
                stopChasing = false;
                //reset the timer so stopchasing isn't activated again immediately
                m_blackboard.searchTime = 0;
            }
        }

        //If the player isn't in sight range and spotted player hasn't been set to false from the timer running out
        if (m_blackboard.notSeenPlayer == true && m_blackboard.spottedPlayer == true)
        {
            //Start counting down on the timer
            countDown();
            m_blackboard.sightReset = false;
        }

        //Always return failure so the script always runs
        if (stopChasing == false)
        {
            return NodeState.FAILURE;
        }

        else
        {
            return NodeState.FAILURE;
        }
    }

    void countDown()
    {
        m_blackboard.searchTime -= Time.deltaTime;
    }

    public override string getName()
    {
        return "PlayerInRange";
    }
}
