using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyNavMesh : BtNode
{
    /// <summary>
    /// Used to get variables from else where (custimisation)
    /// </summary>
    /// 
    private NavMeshAgent m_navMeshAgent;
    private Blackboard m_blackboard;
    private float m_enemySpeed;
    public EnemyNavMesh(NavMeshAgent navMeshAgent, Blackboard blackboard, float enemySpeed)
    {
        m_navMeshAgent = navMeshAgent;
        m_blackboard = blackboard;
        m_enemySpeed = enemySpeed;
    }

    /// <summary>
    /// need to return true or false
    /// </summary>
    /// <param name="blackboard"></param>
    /// <returns></returns>
    public override NodeState evaluate(Blackboard blackboard)
    {
        // Not 100% sure if we said we'd avoid this for now, so maybe just leave it as is I guess? Have a go if you want tho :)

        if (ActivateNavMesh())
        {
            return NodeState.SUCCESS;
        }
        else
        {
            return NodeState.FAILURE;
        }
    }

    private bool ActivateNavMesh()
    {
        m_navMeshAgent.destination = m_blackboard.target.playerObject.transform.position;
        return true;
    }

    /// <summary>
    /// needs to return name of node
    /// </summary>
    /// <returns></returns>
    public override string getName()
    {
        return "Callout";
    }
}
