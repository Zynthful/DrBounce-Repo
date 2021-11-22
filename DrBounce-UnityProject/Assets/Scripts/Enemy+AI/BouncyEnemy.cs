using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncyEnemy : Enemy
{

    private BtNode m_root;
    private Blackboard m_blackboard;

    public bool searching;

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
        BtNode Attack = new Sequence();

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
