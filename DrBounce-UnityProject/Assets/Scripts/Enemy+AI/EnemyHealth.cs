using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;
using MoreMountains.Tools;

public class EnemyHealth : Health
{
    [Header("Feedbacks")]
    public MMFeedbacks DeathFeedback;

    [SerializeField]
    private MMHealthBar _targetHealthBar;

    public delegate void Death();
    public static event Death OnDeath;

    protected override void SetHealth(int value)
    {
        base.SetHealth(value);

        UpdateEnemyHealthBar();
    }

    protected override void DIE()
    {
        base.DIE();

        OnDeath?.Invoke();

        GetComponent<Collider>().enabled = false;
        DeathFeedback?.PlayFeedbacks();
    }

    private void UpdateEnemyHealthBar()
    {
        if (_targetHealthBar != null)
        {
            _targetHealthBar.UpdateBar(health, 0, maxHealth, true);
        }
    }
}
