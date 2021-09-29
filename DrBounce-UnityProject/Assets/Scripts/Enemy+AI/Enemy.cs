using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;

public class Enemy : MonoBehaviour
{
    public float viewDist;
    public float sightAngle;
    public float rateOfFire;
    public BulletType bullet;

    bool shootDelay;

    ObjectPooler pool;

    public enum EnemyTypes
    {
        BlueBack,
        YellowUp,
        RedForward,
    }

    public float health = 5f;

    public void TakeDamage(float amount)
    {
        HitFeedback?.PlayFeedbacks();
        health -= amount;
        if (health <= 0)
        {
            Die();
        }
    }

    public EnemyTypes eType;

    public bool canSeePlayer;

    [Header("Feedbacks")]
    public MMFeedbacks HitFeedback;

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
        
    }

    protected bool PlayerLosCheck()
    {
        if(Vector3.Dot(transform.TransformDirection(Vector3.forward), (PlayerMovement.player.position - transform.position).normalized) > (90 - sightAngle) / 90)
        {
            RaycastHit hit;

            Ray ray = new Ray(transform.position, PlayerMovement.player.position);

            Debug.DrawLine(ray.origin, ray.origin + (PlayerMovement.player.position - transform.position).normalized * viewDist, Color.green);

            if (Physics.Raycast(ray, out hit, viewDist) && hit.transform.CompareTag("Player"))
            {
                Debug.Log("SEE PLAYER IN FRONT OF ME");
                return true;
            }
            else
            {
                Debug.Log(hit.transform.name);
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
            }
            
        }
        return null;
    }

    public void Die()
    {
        print("That's right baby! Our dog, " + this.name + ", is dead!");
        Destroy(gameObject);
    }

    private void Start()
    {
        pool = ObjectPooler.Instance;
        Material mat = GetComponent<MeshRenderer>().material;
        switch (eType)
        {
            case EnemyTypes.BlueBack:
                mat.color = Color.blue;
                break;

            case EnemyTypes.YellowUp:
                mat.color = Color.yellow;
                break;

            case EnemyTypes.RedForward:
                mat.color = Color.red;
                break;
        }
        GetComponent<MeshRenderer>().material = mat;
    }

    IEnumerator ShotDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        shootDelay = false;
    }
}
