using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UnlockSettings", menuName = "ScriptableObjects/Unlocks", order = 1)]
public class Unlocks : ScriptableObject
{
    [Tooltip("Handles unlocks enabled when run")]
    public UnlockTracker.UnlockTypes[] unlocks;

    ///<summary>
    /// Constructor for Unlocks, assigns a list of unlocks to enable when run
    ///</summary>
    ///<param name="i_unlocks">Unlocks to enable, passed into the object</param>
    public Unlocks(UnlockTracker.UnlockTypes[] i_unlocks)
    {
        unlocks = i_unlocks;
    }
}
