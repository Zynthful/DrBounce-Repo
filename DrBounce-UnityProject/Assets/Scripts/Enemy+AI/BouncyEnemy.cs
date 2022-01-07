using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncyEnemy : Enemy
{

    private BtNode m_root;
    private Blackboard m_blackboard;
    private float sightRange;

    public bool searching;
    public bool recentlyAttacked;
    public bool canAttack;
    public bool canMove;
    public int currentTargetIndex;

    // Start is called before the first frame update
    void Start()
    {
        if(m_root == null)
        {
            m_root = createTree();
            m_blackboard = new Blackboard();
            m_blackboard.owner = gameObject;
            m_blackboard.aiController = this;
        }
    }

    protected BtNode createTree()
    {
        BtNode Move = createMovementTree();

        BtNode Attack = createAttackingTree();

        return new Selector(new CheckIfStunned(), Attack, Move);
    }

    protected BtNode createMovementTree()
    {
        BtNode Move;
        if (canMove)
        {
            // Movement Node Section
            BtNode GetPatrolPoint = new Sequence(new IsClose(2f), new TargetNext("PatrolPoint", true));
            BtNode TowardsPatrolPoint = new Sequence(new IsTargeting("PatrolPoint"), new TowardsTarget());
            BtNode UpdatePatrolPoint = new Selector(GetPatrolPoint, TowardsPatrolPoint, new TargetClose("PatrolPoint", true));
            Move = new Sequence(new Inverter(new CheckIfSearching()), UpdatePatrolPoint);
        }
        else
        {
            Move = new Sequence();
        }

        return Move;
    }

    protected BtNode createAttackingTree()
    {
        BtNode Attack;
        if (canAttack)
        {
            // Attack Node Section
            BtNode CanSee = new Selector(new TargetInSight(), new Search());
            BtNode LookAt = new Selector(CanSee, new AfterAttacked());
            BtNode CheckForTarget = new Sequence(new IsClose(sightRange), LookAt, new Callout());
            Attack = new Sequence(new IsNotReloading(), CheckForTarget, new AttackTarget(m_blackboard));
        }
        else
        {
            Attack = new Sequence();
        }

        return Attack;
    }

    // Update is called once per frame
    void Update()
    {
        NodeState result = m_root.evaluate(m_blackboard);
        if(result != NodeState.RUNNING)
        {
            m_root.reset();
        }
    }

    public void ResetRoot()
    {
        m_root.reset();
    }
}
