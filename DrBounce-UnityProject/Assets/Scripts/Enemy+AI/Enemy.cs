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
    protected bool amDead;

    bool shootDelay;

    ObjectPooler pool;

    [Header("Feedbacks")]
    public MMFeedbacks HitFeedback;
    public MMFeedbacks DeathFeedback;

    //need this for floating text
    public MMFeedbackFloatingText HitText;

    public EnemyAudio enemyAudio = null;

    public enum EnemyTypes
    {
        BlueBack,
        YellowUp,
        RedForward,
    }

    public float health = 5f;

    public void TakeDamage(float amount)
    {
        //set hit text value before it is sent off to spawner
        HitText.Value = amount.ToString();
        HitFeedback?.PlayFeedbacks();
        health -= amount;
        if (health <= 0)
        {
            GetComponent<Collider>().enabled = false;
            DeathFeedback?.PlayFeedbacks();
            Die();
        }
    }

    public EnemyTypes eType;

    public bool canSeePlayer;

    

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
        print("That's right baby! Our dog, " + this.name + ", is dead!");
        //Destroy(gameObject);
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
