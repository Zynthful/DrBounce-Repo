using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AfterAttacked : BtNode
{
    /// <summary>
    /// Used to get variables from else where (custimisation)
    /// </summary>
    public AfterAttacked()
    {
    }

    /// <summary>
    /// need to return true or false
    /// </summary>
    /// <param name="blackboard"></param>
    /// <returns></returns>
    public override NodeState evaluate(Blackboard blackboard)
    {
        // If the enemy has recently taken damage, this script should return NodeState.SUCCESS;

        if (blackboard.aiController != null)
        {
            if (blackboard.aiController.recentlyAttacked)
            {
                blackboard.target.NewTarget(true, PlayerMovement.player.gameObject);
                blackboard.owner.transform.rotation = Quaternion.RotateTowards(blackboard.owner.transform.rotation, Quaternion.LookRotation((blackboard.target.playerObject.transform.position - blackboard.owner.transform.position).normalized), Time.deltaTime / .005f);
                return NodeState.SUCCESS;
            }
        }
        else if(blackboard.noBounceAIController != null)
        {
            if (blackboard.noBounceAIController.recentlyAttacked)
            {
                blackboard.target.NewTarget(true, PlayerMovement.player.gameObject);
                blackboard.owner.transform.rotation = Quaternion.RotateTowards(blackboard.owner.transform.rotation, Quaternion.LookRotation((blackboard.target.playerObject.transform.position - blackboard.owner.transform.position).normalized), Time.deltaTime / .005f);
                return NodeState.SUCCESS;
            }
        }
        return NodeState.FAILURE;
    }

    /// <summary>
    /// needs to return name of node
    /// </summary>
    /// <returns></returns>
    public override string getName()
    {
        return "AfterAttacked";
    }
}
