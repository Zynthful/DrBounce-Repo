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
    private bool spottedPlayer;
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

        //Debug.Log("Successfuly reached " + getName());

        if(spottedPlayer == true)
        {
            m_navMeshAgent.destination = PlayerMovement.player.transform.position;
        }

        if (PlayerLosCheck())
        {
            m_blackboard.target.NewTarget(true, PlayerMovement.player.gameObject);
            enemyPosition.rotation = Quaternion.RotateTowards(enemyPosition.rotation, Quaternion.LookRotation((m_blackboard.target.playerObject.transform.position - enemyPosition.position).normalized), Time.deltaTime / .0045f);

            if (m_blackboard.noBounceAIController != null)
            {
                m_blackboard.searchTime = 0;
                spottedPlayer = true;
                //Enables the navmesh if the player is spotted, disables use of waypoints and sets the player as the navmesh target.
                m_blackboard.noBounceAIController.canMove = false;
                m_navMeshAgent.enabled = true;
            }

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
                if (m_blackboard.noBounceAIController != null)
                {
                    //Currently only uses time spent searching to de-activate, should probably also de-activate if the player is too far.
                }
                Debug.DrawLine(ray.origin, ray.origin + (PlayerMovement.player.position - enemyPosition.position).normalized * m_viewDist, Color.red);
            }
        }
        else if(spottedPlayer == true)
        {
            Debug.Log(m_blackboard.searchTime);
            countDown();
        }
        return false;
    }

    void countDown()
    {
        //Counts down from 0 to -5
        m_blackboard.searchTime -= Time.deltaTime;
        if(m_blackboard.searchTime <= -10)
        {
            spottedPlayer = false;
            //resets timer
            m_blackboard.searchTime = 0;
            //disables navmesh if the enemy can't find the player and returns to its waypoints.

            m_navMeshAgent.destination = m_blackboard.noBounceAIController.patrolPoints[0];

            //If enemy's x value is close to the waypoint location
            if(m_blackboard.noBounceAIController.patrolPoints[0].x >= m_blackboard.noBounceAIController.transform.position.x - 5 && m_blackboard.noBounceAIController.patrolPoints[0].x <= m_blackboard.noBounceAIController.transform.position.x + 5)
            {
                m_navMeshAgent.enabled = false;
            }
            m_blackboard.noBounceAIController.canMove = true;
        }
    }
    public override string getName()
    {
        return "TargetInSight";
    }
}
