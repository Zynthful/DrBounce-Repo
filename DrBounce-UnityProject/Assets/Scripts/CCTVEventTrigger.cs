using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;


public class CCTVEventTrigger : MonoBehaviour
{

    [SerializeField] MMFeedbacks CCTVFeedback;
    private bool CCTVPlayed = false;

    void Start()
    {
        CCTVPlayed = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (CCTVPlayed == false)
        {
            CCTVFeedback.PlayFeedbacks();
            CCTVPlayed = true;
        }
        else
        {
            return;
        }
    }
    
    
}
