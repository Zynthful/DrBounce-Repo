using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using MoreMountains.Feedbacks;
using MoreMountains.Tools;

public class Enemy : MonoBehaviour
{
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

    [Header("Declarations")]
    [SerializeField]
    private EnemyAudio enemyAudio = null;
    [SerializeField]
    private EnemyHealth health = null;
    [SerializeField]
    public BulletType bullet;
    [SerializeField]
    private GameObject healthPackPrefab;

    private ObjectPooler pool;

    private bool shootDelay;
    private Coroutine shootingDelayCoroutine;

    public bool canSeePlayer;

    [SerializeField]
    private List<Material> materials = new List<Material> { };

    [Header("Detection Settings")]
    public float viewDist;
    public float sightAngle;
    public float rateOfFire;

    [Header("Events")]
    [SerializeField]
    public UnityEvent onShoot = null;

    /*
    Enemy()
    {
        pool = ObjectPooler.Instance;
    }

    ~Enemy()
    {
        health.DIE();
    }
    */

    protected void FixedUpdate()
    {
        if (!health.GetIsDead())
        {
            //Shoot();
        }
    }

    protected bool PlayerLosCheck()
    {
        if(Vector3.Dot(transform.TransformDirection(Vector3.forward), (PlayerMovement.player.position - transform.position).normalized) > (90 - sightAngle) / 90)
        {
            RaycastHit hit;

            Ray ray = new Ray(transform.position, (PlayerMovement.player.position - transform.position).normalized);

            if (Physics.Raycast(ray, out hit, viewDist) && hit.transform.root.CompareTag("Player"))
            {
                Debug.DrawLine(ray.origin, ray.origin + (PlayerMovement.player.position - transform.position).normalized * viewDist, Color.green);
                return true;
            }
            else
            {
                Debug.DrawLine(ray.origin, ray.origin + (PlayerMovement.player.position - transform.position).normalized * viewDist, Color.red);
            }
        }
        return false;
    }

    protected GameObject Shoot()
    {
        if(PlayerLosCheck())
        {
            if (!shootDelay && shootingDelayCoroutine == null)
            {
                shootDelay = true;
                onShoot?.Invoke();
                shootingDelayCoroutine = StartCoroutine(ShotDelay(rateOfFire));
                pool.SpawnBulletFromPool("Bullet", transform.position, Quaternion.identity, (PlayerMovement.player.position - transform.position).normalized, bullet, null);
                //Debug.Log((PlayerMovement.player.position - transform.position).normalized);
            }
            
        }
        return null;
    }

    /*
    public void Die()
    {
        //SwitchHeldItem.instance.AddToList(Instantiate(healthPack, new Vector3(transform.position.x, transform.position.y + 3, transform.position.z), Quaternion.identity, null));
        print("That's right baby! Our dog, " + this.name + ", is dead!");
        //Destroy(gameObject);
    }
    */

    private void Start()
    {
        pool = ObjectPooler.Instance;
        // Material mat = null;
        // switch (GetComponent<Bouncing>().bType)
        // {
        //     case Bouncing.BounceType.Back:
        //         mat = materials[0];
        //         break;

        //     case Bouncing.BounceType.Up:
        //         mat = materials[1];
        //         break;

        //     case Bouncing.BounceType.Away:
        //         mat = materials[2];
        //         break;
        // }
        // GetComponent<MeshRenderer>().material = mat;
    }

    IEnumerator ShotDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        shootDelay = false;
        shootingDelayCoroutine = null;
    }

    public EnemyAudio GetAudio()
    {
        return enemyAudio;
    }

    /*
    /// <summary>
    /// This and the variable are used for the doors don't delete
    /// </summary>
    /// <returns></returns>
    public bool GetisDead() 
    {
        return amDead;
    }
    */
}
