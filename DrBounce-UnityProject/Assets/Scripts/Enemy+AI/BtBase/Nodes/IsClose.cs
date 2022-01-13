﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsClose : BtNode {
    private float m_distanceLimit = 10;

    public IsClose(float distanceLimit){
        m_distanceLimit = distanceLimit;
    }

    public override NodeState evaluate(Blackboard blackboard) {
        if (blackboard.target == null || blackboard.target == Vector3.zero) {
            return NodeState.FAILURE;
        }

        float distance = (blackboard.owner.transform.localPosition - blackboard.target).magnitude;
        if (distance < m_distanceLimit) {
            return NodeState.SUCCESS;
        } else {
            return NodeState.FAILURE;
        }
    }

    public override string getName()
    {
        return "isClose";
    }

}
