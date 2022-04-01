using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.AI;
using MoreMountains.Feedbacks;
using MoreMountains.Tools;

public class Enemy : MonoBehaviour
{
    #region Declarations
    public class Target
    {
        public Vector3 spottedPosition;
        public bool isPlayer;
        public GameObject playerObject;

        public Target(bool i_isPlayer, GameObject i_playerObject = null, params Vector3[] i_position)
        {
            isPlayer = i_isPlayer;
            if (i_playerObject != null)
            {
                playerObject = i_playerObject;
            }
            if (i_position.Length > 0)
            {
                spottedPosition = i_position[0];
            }
        }

        public void NewTarget(bool i_isPlayer, GameObject i_playerObject = null, params Vector3[] i_position)
        {
            isPlayer = i_isPlayer;
            if (i_playerObject != null)
            {
                playerObject = i_playerObject;
            }
            if (i_position.Length > 0)
            {
                spottedPosition = i_position[0];
            }
        }
    }

    protected BtNode m_root;
    protected Blackboard m_blackboard;

    protected Blackboard.Actions recentAction;

    [Header("Declarations")]
    [SerializeField]
    private EnemyHealth health = null;
    [SerializeField]
    public BulletType bullet;
    [SerializeField]
    private GameObject healthPackPrefab;

    private ObjectPooler pool;

    private bool shootDelay;
    private Coroutine shootingDelayCoroutine;

    public bool recentlyAttacked;
    private Coroutine recentAttackCoroutine;

    public bool canSeePlayer;

    [SerializeField]
    private List<Material> materials = new List<Material> { };

    [Header("Detection Settings")]
    public float viewDist;
    public float sightAngle;
    public float rateOfFire;

    [Header("Events")]
    public UnityEvent onPatrol = null;
    public UnityEvent onAttack = null;
    public UnityEvent onChase = null;
    public UnityEvent onGiveUp = null;
    public UnityEvent onSpotted = null;

    public NavMeshAgent navMeshAgent;

    [Space(10)]
    public List<Vector3> patrolPoints = new List<Vector3> { };

    private int id = 0;

    protected Stun stun;

    [Header("Enemy Data")]
    [SerializeField]
    private EnemyType type = EnemyType.Normal;
    [SerializeField]
    [Tooltip("Name of the enemy. Currently used for Boss HP bars.")]
    new private string name = "";

    public enum EnemyType
    {
        Normal,
        Boss,
    }
    #endregion

    private void Start()
    {
        pool = ObjectPooler.Instance;

        id = gameObject.GetInstanceID();
    }

    protected virtual void Awake()
    {
        stun = GetComponent<Stun>();

        navMeshAgent = GetComponent<NavMeshAgent>();

        foreach (Transform child in transform)
        {
            if (child.tag == "PatrolPoint")
            {
                patrolPoints.Add(child.position);
                Destroy(child.gameObject);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(m_blackboard.currentAction != recentAction)
        {
            switch (m_blackboard.currentAction)
            {
                case Blackboard.Actions.ATTACKING:
                    onAttack?.Invoke();
                    CombatManager.s_Instance.AddEnemy(this);
                    break;

                case Blackboard.Actions.PATROLING:
                    onPatrol?.Invoke();
                    break;

                case Blackboard.Actions.CHASING:
                    onChase?.Invoke();
                    break;

                case Blackboard.Actions.LOST:
                    onGiveUp?.Invoke();
                    CombatManager.s_Instance.RemoveEnemy(this);
                    break;

                case Blackboard.Actions.FIRSTSPOTTED:
                    onSpotted?.Invoke();
                    CombatManager.s_Instance.AddEnemy(this);
                    break;
            }

            recentAction = m_blackboard.currentAction;
        }

        if (!GameManager.s_Instance.paused && m_root != null)
        {
            //Debug.Log(m_blackboard.currentAction);
            NodeState result = m_root.evaluate(m_blackboard);
            if (result != NodeState.RUNNING)
            {
                m_root.reset();
            }
        }
    }

    public void ResetRoot()
    {
        m_root.reset();
    }

    public string GetName() { return name; }
    public EnemyType GetEnemyType() { return type; }
}
