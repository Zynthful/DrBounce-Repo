using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using MoreMountains.Tools;

public class UpdateBossBar : MonoBehaviour
{
    [SerializeField]
    private MMProgressBar healthBar = null;
    [SerializeField]
    private TextMeshProUGUI bossNameText = null;

    private Enemy currentBoss = null;

    [Header("Events")]
    public UnityEvent onEnable;
    public UnityEvent onDisable;

    public void Enable(Enemy boss)
    {
        SetEnabled(boss, true);
    }

    public void Disable(Enemy boss)
    {
        SetEnabled(boss, false);
    }

    public void SetEnabled(Enemy boss, bool value)
    {
        healthBar.enabled = value;
        if (value)
        {
            InitialiseBar(boss);
        }
    }

    private void InitialiseBar(Enemy boss)
    {
        currentBoss = boss;
        bossNameText.text = boss.GetName();

        //healthBar.SetBar(boss.health.GetHealth(), 0.0f, boss.health.GetMaxHealth());  // this doesn't actually set the max value for some reason. i hate you feel.
        healthBar.SetBar01((float)boss.health.GetHealth() / (float)boss.health.GetMaxHealth());
    }

    public void UpdateBar(float value)
    {
        if (currentBoss != null)
        {
            healthBar.UpdateBar01(value / (float)currentBoss.health.GetMaxHealth());
        }
    }
}