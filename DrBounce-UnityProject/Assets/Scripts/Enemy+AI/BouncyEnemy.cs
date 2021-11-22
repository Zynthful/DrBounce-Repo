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
        BtNode Move = new Sequence();

        BtNode CanSee = new Selector(new TargetInSight(), new Search());
        BtNode LookAt = new Selector(CanSee, new AfterAttacked());
        BtNode CheckForTarget = new Sequence(new IsClose(sightRange), LookAt, new Callout());
        BtNode Attack = new Sequence(CheckForTarget, new AttackTarget(m_blackboard));

        return new Sequence(new CheckIfStunned(), Move, Attack);
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
