using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInRange : BtNode
{

    private Blackboard m_blackboard;
    public PlayerInRange(Blackboard blackboard)
    {
        m_blackboard = blackboard;
    }

    public override NodeState evaluate(Blackboard blackboard)
    {
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
