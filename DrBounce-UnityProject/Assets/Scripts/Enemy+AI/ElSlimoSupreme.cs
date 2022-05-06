using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElSlimoSupreme : BouncyEnemy
{
    protected override BtNode createAttackingTree()
    {
        // Attack Node Section
        BtNode CanSee = new Selector(new TargetInSight(m_blackboard, viewDist, sightAngle, visionOrigin));
        BtNode LookAt = new Selector(CanSee, new AfterAttacked());
        BtNode CheckForTarget = new Sequence(LookAt, new IsClose(true, viewDist), new Callout());
        return new Sequence(new CheckBool(1), CheckForTarget, new IsNotReloading(m_blackboard), new AttackTarget(m_blackboard, rateOfFire, bullet));
    }
}
