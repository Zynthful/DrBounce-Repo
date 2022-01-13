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
    public float enemySpeed = 2;

    [Space(10)]
    public List<Vector3> patrolPoints = new List<Vector3> { };

    // Start is called before the first frame update
    void Awake()
    {
        if(m_root == null)
        {
            m_blackboard = new Blackboard();
            m_blackboard.owner = gameObject;
            m_blackboard.aiController = m_blackboard.owner.GetComponent<BouncyEnemy>();
            m_blackboard.target = Vector3.zero;
            m_blackboard.startPosition = Vector3.zero;
            m_root = createTree();
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
        if (canMove)
        {
            // Movement Node Section
            BtNode GetPatrolPoint = new Sequence(new IsClose(.2f), new TargetNext(patrolPoints.ToArray()));
            BtNode TowardsPatrolPoint = new Sequence(new IsTargeting(), new TowardsTarget(enemySpeed));
            BtNode UpdatePatrolPoint = new Selector(GetPatrolPoint, TowardsPatrolPoint, new TargetClose(patrolPoints.ToArray()));
            return new Sequence(new Inverter(new CheckIfSearching()), UpdatePatrolPoint);
        }
        else
        {
            // Empty/returns a fail - TEMP
            return new Sequence(new CheckIfStunned());
        }
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
            // Empty/returns a fail - TEMP
            Attack = new Sequence(new CheckIfStunned());
        }

        return Attack;
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.s_Instance.paused && m_root != null)
        {
            NodeState result = m_root.evaluate(m_blackboard);
            Debug.Log(result);
            if (result != NodeState.RUNNING)
            {
                m_root.reset();
            }
        }
    }

    public void ResetRoot()
    {
        m_root.reset();
    }
}
