using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BounceRequirements", menuName = "ScriptableObjects/BounceReqs", order = 0)]
public class RequirementsForBounce : ScriptableObject
{
    public enum Requirements
    {
        noParent,
        amDead,
        amAlive,
        onlyBounceAgainstEnemies,
        dontBounceAgainstEnemies,
    }

    public Requirements[] requirements;

    public RequirementsForBounce(Requirements[] reqs)
    {
        requirements = reqs;
    }
}
