using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CombatAudioManager : MonoBehaviour
{
    public static CombatAudioManager s_Instance = null;

    [SerializeField]
    private AK.Wwise.State inCombatState = null;
    [SerializeField]
    private AK.Wwise.State outOfCombatState = null;
    [SerializeField]
    private AK.Wwise.RTPC numEnemiesEngaged = null;

    private bool inCombat = false;

    private List<int> enemiesInCombatWith = new List<int>();

    private void Awake()
    {
        if (s_Instance == null)
        {
            s_Instance = FindObjectOfType(typeof(CombatAudioManager)) as CombatAudioManager;
        }

        if (s_Instance == null)
        {
            s_Instance = this;
        }
        else if (s_Instance != this)
        {
            Destroy(gameObject);
        }

        RemoveAllEnemies();
    }

    public void AddEnemy(int enemy)
    {
        if (!enemiesInCombatWith.Contains(enemy))
        {
            enemiesInCombatWith.Add(enemy);
            numEnemiesEngaged.SetGlobalValue(enemiesInCombatWith.Count);
            SetInCombat(true);
        }
    }

    public void RemoveEnemy(int enemy)
    {
        enemiesInCombatWith.Remove(enemy);
        numEnemiesEngaged.SetGlobalValue(enemiesInCombatWith.Count);

        if (enemiesInCombatWith.Count <= 0)
        {
            SetInCombat(false);
        }
    }

    public void RemoveAllEnemies()
    {
        enemiesInCombatWith.Clear();
        numEnemiesEngaged.SetGlobalValue(0);
        SetInCombat(false);
    }

    private void SetInCombat(bool value)
    {
        inCombat = value;
        if (inCombat)
        {
            inCombatState?.SetValue();
        }
        else
        {
            outOfCombatState?.SetValue();
        }
    }

    public bool GetInCombat() { return inCombat; }
}
