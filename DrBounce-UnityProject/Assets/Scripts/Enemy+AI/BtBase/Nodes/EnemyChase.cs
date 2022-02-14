using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class EnemyChase : BtNode
{

    private Blackboard m_blackboard;
    private NavMeshAgent m_navMeshAgent;
    private bool headingBack = false;
    private float m_attackRange;
    private Vector3 targetWaypoint;
    private bool FirstSpotted = false;
    public NavMeshPath path = new NavMeshPath();


    public EnemyChase(Blackboard blackboard, NavMeshAgent navMeshAgent, float attackRange)
    {
        m_blackboard = blackboard;
        m_navMeshAgent = navMeshAgent;
        m_attackRange = attackRange;
    }

    public override NodeState evaluate(Blackboard blackboard)
    {

        Debug.Log(path.status);

        if (m_blackboard.spottedPlayer == true)
        {

            if (PlayerMovement.instance.isGrounded == true)
            {
                //Test if the player can be reached by the AI. Only while grounded so the enemy continues to chase the player even while jumping
                NavMesh.CalculatePath(m_blackboard.noBounceAIController.transform.position, PlayerMovement.instance.groundCheck.position, NavMesh.AllAreas, path);
            }

            if (path.status == NavMeshPathStatus.PathComplete)
            {
                //target the player
                m_navMeshAgent.destination = PlayerMovement.player.transform.position;
                Debug.Log("TESTE");
                m_blackboard.currentAction = Blackboard.Actions.CHASING;

                if(FirstSpotted == false)
                {
                    targetWaypoint = m_blackboard.noBounceAIController.patrolPoints[0];
                    FirstSpotted = true;
                    m_blackboard.noBounceAIController.canMove = false;
                    headingBack = true;
                    m_blackboard.currentAction = Blackboard.Actions.FIRSTSPOTTED;
                }
            }
        }

        if (m_blackboard.searchTime <= -10 || path.status != NavMeshPathStatus.PathComplete && headingBack == true)
        {
            m_blackboard.spottedPlayer = false;
            m_blackboard.noBounceAIController.canMove = true;
            //Move back to nearest waypoint is commented out below, is a bit buggy with new optimised script

            //for (int i = 0; i < m_blackboard.noBounceAIController.patrolPoints.Count; i++)
            //{
            //    Vector3 tempWaypoint = m_blackboard.noBounceAIController.patrolPoints[i];
            //    if (Vector3.Distance(m_blackboard.noBounceAIController.transform.position, tempWaypoint) < Vector3.Distance(m_blackboard.noBounceAIController.transform.position, targetWaypoint))
            //    {
            //        targetWaypoint = tempWaypoint;
            //    }

            //    Debug.Log("Reps of loop");
            //}
            //m_blackboard.target.spottedPosition = targetWaypoint;
            m_navMeshAgent.destination = m_blackboard.target.spottedPosition;
            headingBack = false;
        }



        //BELOW is commented out code that seems to do what the above if statement does. Keeping it here in case something above doesn't work that I missed.

        ////if (Vector3.Distance(m_blackboard.noBounceAIController.transform.position, m_navMeshAgent.destination) <= m_attackRange)
        ////{
        ////    m_blackboard.noBounceAIController.navMeshAgent.destination = m_blackboard.noBounceAIController.transform.position;
        ////    Debug.Log("TESTF");
        ////}

        //if ((m_blackboard.searchTime <= -10 || m_blackboard.noBounceAIController.navMeshAgent.path.status != NavMeshPathStatus.PathComplete) && headingBack == false)
        //{
        //    headingBack = true;
        //    m_blackboard.spottedPlayer = false;

        //    //Set the navmesh destination to the closest patrol point

        //    for (int i = 0; i < m_blackboard.noBounceAIController.patrolPoints.Count; i++)
        //    {
        //        Vector3 tempWaypoint = m_blackboard.noBounceAIController.patrolPoints[i];
        //        if (Vector3.Distance(m_blackboard.noBounceAIController.transform.position, tempWaypoint) < Vector3.Distance(m_blackboard.noBounceAIController.transform.position, targetWaypoint))
        //        {
        //            targetWaypoint = tempWaypoint;
        //        }

        //        Debug.Log("Reps of loop");
        //    }
        //    Debug.Log(targetWaypoint);
        //    m_blackboard.noBounceAIController.navMeshAgent.destination = targetWaypoint;
        //    m_blackboard.currentAction = Blackboard.Actions.LOST;

        //    Debug.Log("TESTD");
        //    //Once the patrol point has been reached, or the enemy is close enough to it
        //    //If enemy's x value is close to the waypoint location
        //}

        //if (Vector3.Distance(m_blackboard.noBounceAIController.transform.position, targetWaypoint) <= 2.5f && headingBack == true)
        //{
        //    //Disable the timer, navmesh, set stopchasing to false, allowing the enemy to target the player again if spotted
        //    //resets timer
        //    m_blackboard.searchTime = 0;
        //    //Sets the closest waypoint as its target position, as well as making it the next destination in the list - to allow it to move to the next one in the sequence once it gets there
        //    m_blackboard.target.spottedPosition = targetWaypoint;
        //    m_navMeshAgent.destination = m_blackboard.target.spottedPosition;
        //    m_blackboard.noBounceAIController.canMove = true;
        //    headingBack = false;
        //    Debug.Log("TESTC");
        //}

        //If the player isn't in sight range and spottedPlayer hasn't been set to false from the timer running out
        if (m_blackboard.notSeenPlayer == true && m_blackboard.spottedPlayer == true)
        {
            //Start counting down on the timer
            countDown();
            Debug.Log("TESTB");
        }

        //Always return failure so the script always runs
        return NodeState.FAILURE;
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
