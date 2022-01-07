using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BtController : MonoBehaviour
{
    private BtNode m_root;
    private Blackboard m_blackboard;
    private Material startingMat;
    private MeshRenderer mesh_r;

    private GameObject startingPos;

    // method to create the tree, sorry - no GUI for this we need to build it by hand
    protected BtNode createTree()
    {
        return new Sequence(new IsTargeting(), new TowardsTarget());
    }

    // Start is called before the first frame update
    void Start() 
    {
        if (m_root == null)
        {
            startingPos = new GameObject("StartingPosition" + gameObject.name);
            startingPos.transform.position = transform.position;
            mesh_r = GetComponent<MeshRenderer>();
            m_blackboard = new Blackboard();
            m_blackboard.owner = gameObject;
            m_root = createTree();
            startingMat = mesh_r.material;
        }
    }

    // Update is called once per frame
    void Update() 
    {
        NodeState result = m_root.evaluate(m_blackboard);
        if ( result != NodeState.RUNNING ) {
            m_root.reset();
        }
    }

    /// <summary>
    /// This is used to reset the root node of the AI tree to re-evaluate what it's current task should be
    /// </summary>
    public void ResetRoot()
    {
        m_root.reset();
    }
}
