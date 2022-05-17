using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

[CreateAssetMenu(fileName = "New Unlock", menuName = "ScriptableObjects/Unlocks/Unlocks", order = 1)]
public class Unlocks : ScriptableObject
{
    [Tooltip("Handles unlocks enabled when run")]
    public UnlockTracker.UnlockTypes[] unlocks;

    [Header("UI Popup Info")]
    new public string name;
    public VideoClip clip;

    ///<summary>
    /// Constructor for Unlocks, assigns a list of unlocks to enable when run
    ///</summary>
    ///<param name="i_unlocks">Unlocks to enable, passed into the object</param>
    public Unlocks(UnlockTracker.UnlockTypes[] i_unlocks, string i_name, VideoClip i_clip)
    {
        unlocks = i_unlocks;
        name = i_name;
        clip = i_clip;
    }
}
