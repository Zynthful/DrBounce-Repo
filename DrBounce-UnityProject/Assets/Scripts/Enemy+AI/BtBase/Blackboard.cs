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

    public float searchTime;
    public bool spottedPlayer;
    public bool sightReset;
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
