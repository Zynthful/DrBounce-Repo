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
    public EnemyHealth health = null;
    public BulletType bullet;
    [SerializeField]
    private GameObject healthPackPrefab;

    private ObjectPooler pool;

    private bool shootDelay;
    private Coroutine shootingDelayCoroutine;

    public bool recentlyAttacked;

    protected bool rotateToDefault = false;

    protected Vector3 defaultRotation;
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
    public EnemyType GetEnemyType() { return type; }

    public enum EnemyType
    {
        Normal,
        Boss,
    }

    [SerializeField]
    [Tooltip("Name of the enemy. Currently used for Boss HP bars.")]
    new private string name = "";
    public string GetName() { return name; }

    [SerializeField]
    [Tooltip("The text displayed when the enemy name has not been revealed.")]
    private string unknownName = "???";
    public string GetUnknowneName() { return unknownName; }

    [SerializeField]
    [Tooltip("If this is greater than 0 and this Enemy is a Boss Enemy, the name will be hidden as '???' until this delay has passed.")]
    private float nameChangeDelay = 0;
    public float GetNameDelay() { return nameChangeDelay; }

    [SerializeField]
    [Tooltip("Whether to remove this enemy from our list of enemies in combat with if this enemy loses track of its target.")]
    private bool removeFromCombatWhenLost = true;
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

        defaultRotation = transform.rotation.eulerAngles;

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
    protected virtual void Update()
    {
        if (m_blackboard.shotDelay >= 0)
            m_blackboard.shotDelay -= Time.deltaTime;

        if (m_blackboard.currentAction != recentAction)
        {
            switch (m_blackboard.currentAction)
            {
                case Blackboard.Actions.ATTACKING:
                    rotateToDefault = false;
                    onAttack?.Invoke();
                    CombatManager.s_Instance.AddEnemy(this);
                    break;

                case Blackboard.Actions.PATROLING:
                    rotateToDefault = false;
                    onPatrol?.Invoke();
                    break;

                case Blackboard.Actions.CHASING:
                    rotateToDefault = false;
                    onChase?.Invoke();
                    break;

                case Blackboard.Actions.LOST:
                    rotateToDefault = true;
                    onGiveUp?.Invoke();
                    if (removeFromCombatWhenLost)
                        CombatManager.s_Instance.RemoveEnemy(this);
                    break;

                case Blackboard.Actions.FIRSTSPOTTED:
                    rotateToDefault = false;
                    onSpotted?.Invoke();
                    CombatManager.s_Instance.AddEnemy(this);
                    break;
            }

            recentAction = m_blackboard.currentAction;
        }

        if(rotateToDefault && !recentlyAttacked)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(defaultRotation), Time.deltaTime / .01f);
        }

        if (!GameManager.s_Instance.paused && m_root != null && !health.GetIsDead())
        {
            //Debug.Log(m_blackboard.currentAction);
            NodeState result = m_root.evaluate(m_blackboard);
            if (result != NodeState.RUNNING)
            {
                m_root.reset();
            }
        }
    }

    private void OnDisable() 
    {
        m_blackboard.currentAction = Blackboard.Actions.NONE;
        CombatManager.s_Instance.RemoveEnemy(this);
    }

    public void ResetRoot()
    {
        m_root.reset();
    }
}
