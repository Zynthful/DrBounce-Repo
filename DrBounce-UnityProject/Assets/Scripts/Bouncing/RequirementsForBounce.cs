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

    [SerializeField]
    [Tooltip("Cooldown before bouncing off of the same object again is counted as a 'unique' bounce (e.g., granting charges)")]
    private float sameBounceCooldown = 2.0f;
    public float GetSameBounceCooldown() { return sameBounceCooldown; }
    public void SetSameBounceCooldown(float value) { sameBounceCooldown = value; }

    public RequirementsForBounce(Requirements[] reqs)
    {
        requirements = reqs;
    }
}
