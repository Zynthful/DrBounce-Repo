using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Search : BtNode
{
    /// <summary>
    /// Used to get variables from else where (custimisation)
    /// </summary>
    public Search()
    {
    }

    private Transform enemyRotation;

    /// <summary>
    /// need to return true or false
    /// </summary>
    /// <param name="blackboard"></param>
    /// <returns></returns>


    public override NodeState evaluate(Blackboard blackboard)
    {
        enemyRotation = blackboard.owner.transform;
        // Rotate the enemy to look around for the player, the TargetInSight node should take care of spotting the player once rotated

        if (Rotate())
        {
            Debug.Log("Searching");
            return NodeState.FAILURE;
        }
        else
        {
            return NodeState.FAILURE;
        }
    }

    private bool Rotate()
    {
        enemyRotation.rotation = Quaternion.RotateTowards(enemyRotation.rotation, enemyRotation.rotation * Quaternion.Euler(0,1000,0), Time.deltaTime / .0045f);
        return true;
    }

    /// <summary>
    /// needs to return name of node
    /// </summary>
    /// <returns></returns>
    public override string getName()
    {
        return "Search";
    }
}
