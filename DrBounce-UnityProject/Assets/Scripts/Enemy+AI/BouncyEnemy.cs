using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncyEnemy : Enemy
{
    public bool searching;
    public bool canAttack;
    public bool canMove;
    public int currentTargetIndex;
    public float enemySpeed = 2;

    public Vector3 visionOrigin;

    // Start is called before the first frame update
    protected override void Awake()
    {
        base.Awake();
        if(m_root == null)
        {
            m_blackboard = new Blackboard();
            m_blackboard.owner = gameObject;
            m_blackboard.aiController = m_blackboard.owner.GetComponent<BouncyEnemy>();
            m_blackboard.target = new Target(false, null, Vector3.zero);
            m_blackboard.startPosition = Vector3.zero;
            m_root = createTree();
        }
    }

    protected BtNode createTree()
    {
        BtNode Move = createMovementTree();

        BtNode Attack = createAttackingTree();

        return new Selector(new CheckIfStunned(stun), Attack, Move);
    }

    private void OnEnable() 
    {
        if(m_root == null)
        {
            createTree();
        }
    }

    protected virtual BtNode createMovementTree()
    {
        // Movement Node Section
        BtNode GetPatrolPoint = new Sequence(new IsClose(false, .2f), new TargetNext(patrolPoints.ToArray()));
        BtNode TowardsPatrolPoint = new Sequence(new IsTargeting(false), new TowardsTarget(navMeshAgent, enemySpeed));
        BtNode UpdatePatrolPoint = new Selector(GetPatrolPoint, TowardsPatrolPoint, new TargetClose(patrolPoints.ToArray()));
        return new Sequence(new CheckBool(0), new Inverter(new CheckIfSearching()), UpdatePatrolPoint);
    }

    protected virtual BtNode createAttackingTree()
    {
        // Attack Node Section
        BtNode CanSee = new Selector(new TargetInSight(m_blackboard, viewDist, sightAngle, visionOrigin));
        BtNode LookAt = new Selector(CanSee, new AfterAttacked());
        BtNode CheckForTarget = new Sequence(LookAt, new IsClose(true, viewDist), new Callout());
        return new Sequence(new CheckBool(1), CheckForTarget, new IsNotReloading(m_blackboard), new AttackTarget(m_blackboard, rateOfFire, bullet, visionOrigin));
    }
}
