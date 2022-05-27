using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ElSlimoSupreme : BouncyEnemy
{

    [SerializeField] private BulletType chargedShotType;

    [SerializeField] private float chargedShotDelay;

    public UnityEvent chargingShot = null;
    public UnityEvent stopCharging = null;

    private bool chargingShotEvent = true;

    [SerializeField] [Range(0, 8)] [Tooltip("Time before firing a charge shot that ")]
    private float triggerChargeEventTime;

    protected override BtNode createAttackingTree()
    {
        // Attack Node Section
        BtNode CanSee = new Selector(new TargetInSight(m_blackboard, viewDist, sightAngle, visionOrigin));
        BtNode LookAt = new Selector(CanSee, new AfterAttacked());
        BtNode CheckForTarget = new Sequence(LookAt, new IsClose(true, viewDist), new Callout());
        return new Sequence(new CheckBool(1), CheckForTarget, new IsNotReloading(m_blackboard), new ElSlimoAttackTarget(m_blackboard, rateOfFire, chargedShotDelay, bullet, chargedShotType, visionOrigin));
    }

    protected override void Update()
    {
        base.Update();

        if(triggerChargeEventTime > chargedShotDelay-1)
            triggerChargeEventTime = chargedShotDelay-1;

        if(m_blackboard.chargedShotDelay >= 0)
            m_blackboard.chargedShotDelay -= Time.deltaTime;

        if(m_blackboard.chargedShotDelay > chargedShotDelay - 1 && chargingShotEvent)
        {
            chargingShotEvent = false;
            stopCharging?.Invoke();
        }

        if(m_blackboard.chargedShotDelay < triggerChargeEventTime && !chargingShotEvent)
        {
            chargingShotEvent = true;
            chargingShot?.Invoke();
        }
    }
}
