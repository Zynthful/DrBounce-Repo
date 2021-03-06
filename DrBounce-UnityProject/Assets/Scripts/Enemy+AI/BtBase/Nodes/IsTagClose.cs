using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class IsTagClose : BtNode
{
    private float m_distanceLimit = 10; private float m_distanceMin = 0;
    private string m_tag;
    private bool m_moveTowards;
    private GameObject newMarker;
    private NavMeshAgent m_agent;
    private GameObject[] m_ignoreObjs;

    private GameObject player;

    public IsTagClose(float distanceLimit, string tag, bool moveTowards, float distanceMin, params GameObject[] ignoreObjs)
    {
        m_distanceLimit = distanceLimit;
        m_tag = tag;
        m_moveTowards = moveTowards;
        m_distanceMin = distanceMin;
        m_ignoreObjs = ignoreObjs;

        // If the AI is trying to move away from the closest matching target, then it requires a marker to attempt to move towards
        if (!m_moveTowards)
        {
            newMarker = new GameObject("AvoidMarker");
        }
    }

    public override NodeState evaluate(Blackboard blackboard)
    {
        if (m_agent == null)
        {
            m_agent = blackboard.owner.GetComponent<NavMeshAgent>();
        }

        double closeDist = double.PositiveInfinity;
        Vector3 closest = Vector3.zero;

        if (m_tag == "Player")
        {
            if(player == null)
                player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                closeDist = Vector3.Distance(player.transform.position, blackboard.owner.transform.position);
                closest = player.transform.position;
            }
        }
        else
        {
            GameObject[] objects = GameObject.FindGameObjectsWithTag(m_tag);

            // Check each possible tagged object against the list of objects to ignore
            foreach (GameObject obj in objects)
            {
                bool allowObj = true;

                foreach (GameObject ignore in m_ignoreObjs)
                {
                    if (obj == ignore)
                    {
                        allowObj = false;
                    }
                }

                // If the object isn't in the ignore list, then the object can be checked against the closest distance
                if (allowObj)
                {
                    double distance = Vector3.Distance(obj.transform.position, blackboard.owner.transform.position);
                    if (distance < closeDist)
                    {
                        closeDist = distance;
                        closest = obj.transform.position;
                    }
                }
            }
        }

        if (closeDist <= m_distanceLimit && closeDist >= m_distanceMin && closest != null)
        {
            return NodeState.SUCCESS;
        }
        else
        {
            return NodeState.FAILURE;
        }
    }

    public override string getName()
    {
        return "isTagClose";
    }

}

