using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class EnemyChase : BtNode
{

    private Blackboard m_blackboard;
    private NavMeshAgent m_navMeshAgent;
    private bool stopChasing = false;
    private NavMeshPath path = new NavMeshPath();
    private bool headingBack = false;
    private float m_attackRange;
    private Vector3 targetWaypoint;

    public EnemyChase(Blackboard blackboard, NavMeshAgent navMeshAgent, float attackRange)
    {
        m_blackboard = blackboard;
        m_navMeshAgent = navMeshAgent;
        m_attackRange = attackRange;
    }

    public override NodeState evaluate(Blackboard blackboard)
    {
        NavMesh.CalculatePath(m_blackboard.noBounceAIController.transform.position, PlayerMovement.instance.groundCheck.position, NavMesh.AllAreas, path);
        Debug.Log(path.status);

        if (m_blackboard.spottedPlayer == true && stopChasing == false && path.status != NavMeshPathStatus.PathPartial)
        {
            //target the player
            m_navMeshAgent.destination = PlayerMovement.player.transform.position;
            targetWaypoint = m_blackboard.noBounceAIController.patrolPoints[0];
            m_blackboard.currentAction = Blackboard.Actions.CHASING;
            m_blackboard.noBounceAIController.canMove = false;
            Debug.Log("Test");
        }

        if (Vector3.Distance(m_blackboard.noBounceAIController.transform.position, m_navMeshAgent.destination) <= m_attackRange)
        {
            m_blackboard.noBounceAIController.navMeshAgent.destination = m_blackboard.noBounceAIController.transform.position;
        }

        if ((m_blackboard.searchTime <= -10 || m_blackboard.noBounceAIController.navMeshAgent.path.status != NavMeshPathStatus.PathComplete) && headingBack == false)
        {
            headingBack = true;
            stopChasing = true;
            m_blackboard.spottedPlayer = false;

            //Set the navmesh destination to the closest patrol point

            for (int i = 0; i < m_blackboard.noBounceAIController.patrolPoints.Count; i++)
            {
                Vector3 tempWaypoint = m_blackboard.noBounceAIController.patrolPoints[i];
                if (Vector3.Distance(m_blackboard.noBounceAIController.transform.position, tempWaypoint) < Vector3.Distance(m_blackboard.noBounceAIController.transform.position, targetWaypoint))
                {
                    targetWaypoint = tempWaypoint;
                }

                Debug.Log("Reps of loop");
            }
            Debug.Log(targetWaypoint);
            m_blackboard.noBounceAIController.navMeshAgent.destination = targetWaypoint;
            m_blackboard.currentAction = Blackboard.Actions.LOST;

            //Once the patrol point has been reached, or the enemy is close enough to it
            //If enemy's x value is close to the waypoint location
        }

        if (Vector3.Distance(m_blackboard.noBounceAIController.transform.position, targetWaypoint) <= 7.5f && headingBack == true)
        {
            //Disable the timer, navmesh, set stopchasing to false, allowing the enemy to target the player again if spotted
            stopChasing = false;
            //resets timer
            m_blackboard.searchTime = 0;
            m_blackboard.target.spottedPosition = targetWaypoint;
            m_navMeshAgent.destination = m_blackboard.target.spottedPosition;
            m_blackboard.noBounceAIController.canMove = true;
            headingBack = false;
        }

        //This section allows the enemy to re-target the player if they're seen while travelling back to a waypoint

        //If the chase timer has run out or the player has gone out of reach, but if the player can still be seen
        if (stopChasing == true && m_blackboard.notSeenPlayer == false)
        {
            //Test if the player can be reached by the enemy
            

            //If they can:
            if (path.status == NavMeshPathStatus.PathComplete)
            {
                //Set stop chasing to false, allowing the player to be chased again if spotted
                stopChasing = false;
                //reset the timer so stopchasing isn't activated again immediately
                m_blackboard.searchTime = 0;
            }
        }

        //If the player isn't in sight range and spottedPlayer hasn't been set to false from the timer running out
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
