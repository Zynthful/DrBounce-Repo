using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blackboard {
    public GameObject owner;
    public Enemy.Target target;
    public Vector3 startPosition;
    public BouncyEnemy aiController;

    public bool HasTarget()
    {
        if (target != null)
        {
            if (target.isPlayer && target.playerObject != null)
            {
                return true;
            }
            else if (!target.isPlayer && target.spottedPosition != Vector3.zero)
            {
                return true;
            }
        }
        return false;
    }
}
