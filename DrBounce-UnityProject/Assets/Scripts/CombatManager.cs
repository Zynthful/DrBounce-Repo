using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CombatManager : MonoBehaviour
{
    public static CombatManager s_Instance = null;

    private bool inCombat = false;
    private List<Enemy> enemiesInCombatWith = new List<Enemy>();

    [Header("Events")]
    public GameEvent onEnterCombat = null;
    public GameEvent onExitCombat = null;
    public GameEventEnemy onEnterCombatWithEnemy = null;
    public GameEventEnemy onEnterCombatWithBossEnemy = null;
    public GameEventEnemy onEnterCombatWithNormalEnemy = null;
    public GameEventEnemy onExitCombatWithEnemy = null;
    public GameEventEnemy onExitCombatWithBossEnemy = null;
    public GameEventEnemy onExitCombatWithNormalEnemy = null;

    [Header("Wwise")]
    [SerializeField]
    private AK.Wwise.RTPC numEnemiesEngaged = null;

    private void Awake()
    {
        if (s_Instance == null)
        {
            s_Instance = FindObjectOfType(typeof(CombatManager)) as CombatManager;
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

    public void AddEnemy(Enemy enemy)
    {
        if (!enemiesInCombatWith.Contains(enemy))
        {
            enemiesInCombatWith.Add(enemy);
            numEnemiesEngaged.SetGlobalValue(enemiesInCombatWith.Count);
            SetInCombat(true);

            onEnterCombatWithEnemy?.Raise(enemy);

            switch (enemy.GetEnemyType())
            {
                case Enemy.EnemyType.Normal:
                    onEnterCombatWithNormalEnemy?.Raise(enemy);
                    break;
                case Enemy.EnemyType.Boss:
                    onEnterCombatWithBossEnemy?.Raise(enemy);
                    break;
                default:
                    break;
            }
        }
    }

    public void RemoveEnemy(Enemy enemy)
    {
        enemiesInCombatWith.Remove(enemy);
        numEnemiesEngaged.SetGlobalValue(enemiesInCombatWith.Count);

        onExitCombatWithEnemy?.Raise(enemy);

        if (enemiesInCombatWith.Count <= 0)
        {
            SetInCombat(false);
        }

        switch (enemy.GetEnemyType())
        {
            case Enemy.EnemyType.Normal:
                onExitCombatWithNormalEnemy?.Raise(enemy);
                break;
            case Enemy.EnemyType.Boss:
                onExitCombatWithBossEnemy?.Raise(enemy);
                break;
            default:
                break;
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
            onEnterCombat?.Raise();
        }
        else
        {
            onExitCombat?.Raise();
        }
    }

    public bool GetInCombat() { return inCombat; }
}
