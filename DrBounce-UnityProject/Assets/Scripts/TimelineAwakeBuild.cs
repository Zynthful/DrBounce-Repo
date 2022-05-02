using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TimelineAwakeBuild : MonoBehaviour
{
    private PlayableDirector introDirector;
    // Start is called before the first frame update


    void Awake()
    {
        introDirector = GetComponent<PlayableDirector>();
        introDirector.Evaluate();
    }

}
