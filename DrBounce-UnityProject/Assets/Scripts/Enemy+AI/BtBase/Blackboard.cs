using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blackboard {
    public GameObject owner;
    public Enemy.Target target;
    public Vector3 startPosition;
    public BouncyEnemy aiController;
    public NoBounceEnemy noBounceAIController;
    public float shotDelay;

    public enum Actions
    {
        NONE,
        ATTACKING,
        PATROLING,
        CHASING,
        LOST,
    }

    public Actions currentAction;

    //The amount of time the enemy chases the player for after losing their location
    public float searchTime;
    //Activates if the player has been spotted, deactivates if searchtime hits -10
    public bool spottedPlayer;
    //a bool in TargetInSight that allows the timer to be reset once after the enemy has spotted the player, and only reset once the enemy is chasing the player and isn't in sight range.
    public bool sightReset;
    //Only active if the player is seen by the enemy in TargetInSight, sent to EnemyChase for an accurate searchTime Countdown
    public bool notSeenPlayer;

    public bool HasTarget(bool player)
    {
        if (target != null)
        {
            if (player)
            {
                if (target.isPlayer && target.playerObject != null)
                {
                    return true;
                }
            }
            else if (target.spottedPosition != Vector3.zero)
            {
                return true;
            }
        }
        return false;
    }
}
