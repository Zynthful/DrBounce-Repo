using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;
using MoreMountains.Tools;

public class EnemyHealth : Health
{
    [Header("Feedbacks")]
    public MMFeedbacks DeathFeedback;

    public delegate void Death();
    public static event Death OnDeath;

    protected override void DIE()
    {
        base.DIE();

        OnDeath?.Invoke();

        GetComponent<Collider>().enabled = false;
        DeathFeedback?.PlayFeedbacks();
    }
}
