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
    [SerializeField]
    public UnityEvent onPatrol = null;
    [SerializeField]
    public UnityEvent onAttack = null;
    [SerializeField]
    public UnityEvent onChase = null;
    [SerializeField]
    public UnityEvent onGiveUp = null;
    [SerializeField]
    public UnityEvent onSpotted = null;

    public NavMeshAgent navMeshAgent;

    [Space(10)]
    public List<Vector3> patrolPoints = new List<Vector3> { };

    protected Stun stun;
    #endregion

    private void Start()
    {
        pool = ObjectPooler.Instance;
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
                    break;

                case Blackboard.Actions.PATROLING:
                    onPatrol?.Invoke();
                    break;

                case Blackboard.Actions.CHASING:
                    onChase?.Invoke();
                    break;

                case Blackboard.Actions.LOST:
                    onGiveUp?.Invoke();
                    break;

                case Blackboard.Actions.FIRSTSPOTTED:
                    onSpotted?.Invoke();
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
}
