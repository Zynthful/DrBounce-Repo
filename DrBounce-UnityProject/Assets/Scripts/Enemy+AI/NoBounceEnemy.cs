using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoBounceEnemy : Enemy
{
    public bool searching;
    public bool canAttack;
    public bool canMove;
    public int currentTargetIndex;
    public float enemySpeed = 2;
    public float attackRange = 2;
    public float attackDelay = .75f;
    public int contactDamage;

    // Start is called before the first frame update
    protected override void Awake()
    {
        base.Awake();
        if(m_root == null)
        {
            m_blackboard = new Blackboard();
            m_blackboard.owner = gameObject;
            m_blackboard.noBounceAIController = m_blackboard.owner.GetComponent<NoBounceEnemy>();
            m_blackboard.aiController = null;
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

    protected BtNode createMovementTree()
    {
        // Movement Node Section
        BtNode GetPatrolPoint = new Sequence(new IsClose(false, .2f), new TargetNext(patrolPoints.ToArray()));
        BtNode TowardsPatrolPoint = new Sequence(new IsTargeting(false), new TowardsTarget(enemySpeed));
        BtNode UpdatePatrolPoint = new Selector(GetPatrolPoint, TowardsPatrolPoint, new TargetClose(patrolPoints.ToArray()));
        return new Sequence(new CheckBool(3), new Inverter(new CheckIfSearching()), UpdatePatrolPoint);
    }

    protected BtNode createAttackingTree()
    {
        // Attack Node Section
        BtNode CanSee = new Selector(new EnemyChase(m_blackboard, navMeshAgent, attackRange), new TargetInSight(m_blackboard, viewDist, sightAngle));
        BtNode LookAt = new Selector(CanSee, new AfterAttacked());
        BtNode CheckForTarget = new Sequence(LookAt, new IsClose(true, viewDist), new Callout());
        BtNode AttackTarget = new Sequence(new IsNotReloading(m_blackboard), new IsClose(true, attackRange), new MeleeAttackTarget(attackDelay, contactDamage));
        return new Sequence(new CheckBool(4), CheckForTarget, AttackTarget);
    }
}
