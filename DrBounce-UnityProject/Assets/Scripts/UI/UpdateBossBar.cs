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

    [SerializeField]
    private GameObject[] barObjects = null;
    private Enemy currentBoss = null;

    [Header("Events")]
    public UnityEvent onEnable;
    public UnityEvent onDisable;

    private void Awake()
    {
        Disable(currentBoss);
    }

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
        for (int i = 0; i < barObjects.Length; i++)
        {
            barObjects[i].SetActive(value);
        }

        if (boss == null)
            return;

        if (value)
        {
            onEnable.Invoke();
            InitialiseBar(boss);
        }
        else
        {
            onDisable.Invoke();
        }
    }

    private void InitialiseBar(Enemy boss)
    {
        currentBoss = boss;

        if (boss.GetNameDelay() > 0)
            StartCoroutine(DelayNameChange(boss));
        else
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

    private IEnumerator DelayNameChange(Enemy boss)
    {
        bossNameText.text = boss.GetUnknowneName();
        yield return new WaitForSeconds(boss.GetNameDelay());
        bossNameText.text = boss.GetName();
    }
}