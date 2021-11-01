using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;
using MoreMountains.Tools;

public class Enemy : MonoBehaviour
{
    public float viewDist;
    public float sightAngle;
    public float rateOfFire;
    public BulletType bullet;
    protected bool amDead;
    protected MMHealthBar _targetHealthBar;

    bool shootDelay;

    [SerializeField] private GameObject healthPack;

    ObjectPooler pool;

    public delegate void Death();
    public static event Death OnDeath;

    [Header("Feedbacks")]
    public MMFeedbacks HitFeedback;
    public MMFeedbacks DeathFeedback;

    //need this for floating text
    public MMFeedbackFloatingText HitText;

    public EnemyAudio enemyAudio = null;

    protected float _minimumHealth = 0f;
    [SerializeField] protected float _maximumHealth = 20f;
    protected float _currentHealth = 5f;

    public void TakeDamage(float amount)
    {
        //set hit text value before it is sent off to spawner
        HitText.Value = amount.ToString();
        HitFeedback?.PlayFeedbacks();
        _currentHealth -= amount;
        UpdateEnemyHealthBar();
        if (_currentHealth <= 0)
        {
            GetComponent<Collider>().enabled = false;
            DeathFeedback?.PlayFeedbacks();
            Die();
        }
    }

    public bool canSeePlayer;

    [SerializeField] List<Material> materials = new List<Material>{};

    Enemy()
    {
        pool = ObjectPooler.Instance;
    }

    ~Enemy()
    {
        Die();
    }

    protected void FixedUpdate()
    {
        if (!amDead)
            Shoot();
    }

    protected bool PlayerLosCheck()
    {
        if(Vector3.Dot(transform.TransformDirection(Vector3.forward), (PlayerMovement.player.position - transform.position).normalized) > (90 - sightAngle) / 90)
        {
            RaycastHit hit;

            Ray ray = new Ray(transform.position, (PlayerMovement.player.position - transform.position).normalized);

            if (Physics.Raycast(ray, out hit, viewDist) && hit.transform.CompareTag("Player"))
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
            if(!shootDelay)
            {
                shootDelay = true;
                StartCoroutine(ShotDelay(rateOfFire));
                pool.SpawnBulletFromPool("Bullet", transform.position, Quaternion.identity, (PlayerMovement.player.position - transform.position).normalized, bullet, null);
                Debug.Log((PlayerMovement.player.position - transform.position).normalized);
            }
            
        }
        return null;
    }

    public void Die()
    {
        amDead = true;
        OnDeath?.Invoke();
        //SwitchHeldItem.instance.AddToList(Instantiate(healthPack, new Vector3(transform.position.x, transform.position.y + 3, transform.position.z), Quaternion.identity, null));
        print("That's right baby! Our dog, " + this.name + ", is dead!");
        //Destroy(gameObject);
    }

    private void Start()
    {
        _currentHealth = _maximumHealth;
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

    private void Awake()
    {
        _targetHealthBar = this.gameObject.GetComponent<MMHealthBar>();
    }

    public virtual void UpdateEnemyHealthBar()
    {
        if (_targetHealthBar != null)
        {
            _targetHealthBar.UpdateBar(_currentHealth, _minimumHealth, _maximumHealth, true);
        }
    }

    IEnumerator ShotDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        shootDelay = false;
    }
}
