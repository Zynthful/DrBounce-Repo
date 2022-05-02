using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CombatManager : MonoBehaviour
{
    public static CombatManager s_Instance = null;

    private List<Enemy> enemiesInCombatWith = new List<Enemy>();

    private Coroutine delayCoroutine = null;

    private bool inCombat = false;
    public bool GetInCombat() { return inCombat; }
    private void SetInCombat(bool value)
    {
        if (inCombat == value)
            return;

        inCombat = value;
        if (value)
        {
            if (delayCoroutine != null)
            {
                StopCoroutine(delayCoroutine);
                delayCoroutine = null;
            }
            onEnterCombat?.Raise();
        }
        else
        {
            onExitCombat?.Raise();
        }
    }

    [Header("Combat Settings")]
    [SerializeField]
    [Tooltip("The duration to wait when there are no enemies in combat with before considering the player as having exited combat.")]
    private float outOfCombatDelay = 5.0f;

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

        RemoveAllEnemies(true);
        onExitCombat?.Raise();
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

    /// <summary>
    /// 
    /// </summary>
    /// <param name="enemy"></param>
    /// <param name="ignoreDelay"></param>
    public void RemoveEnemy(Enemy enemy, bool ignoreDelay = false)
    {
        enemiesInCombatWith.Remove(enemy);
        numEnemiesEngaged.SetGlobalValue(enemiesInCombatWith.Count);

        onExitCombatWithEnemy?.Raise(enemy);

        if (enemiesInCombatWith.Count <= 0)
        {
            if (ignoreDelay)
                SetInCombat(false);
            else
                delayCoroutine = StartCoroutine(DelayOutOfCombat());
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

    /// <summary>
    /// 
    /// </summary>
    /// <param name="ignoreDelay"></param>
    public void RemoveAllEnemies(bool ignoreDelay = false)
    {
        foreach (Enemy enemy in enemiesInCombatWith)
        {
            RemoveEnemy(enemy, ignoreDelay);
        }

        // Failsafe if we didn't get rid of all enemies before
        if (enemiesInCombatWith.Count >= 1)
        {
            enemiesInCombatWith.Clear();
            numEnemiesEngaged.SetGlobalValue(0);

            if (ignoreDelay)
                SetInCombat(false);
            else
                delayCoroutine = StartCoroutine(DelayOutOfCombat());
        }
    }

    private IEnumerator DelayOutOfCombat()
    {
        yield return new WaitForSeconds(outOfCombatDelay);

        // Make sure we're still out of combat
        if (enemiesInCombatWith.Count <= 0)
            SetInCombat(false);
    }
}
