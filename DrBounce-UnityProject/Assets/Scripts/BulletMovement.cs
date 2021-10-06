using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;

public class BulletMovement : MonoBehaviour, IPooledObject
{

    protected Rigidbody rb;
    public float speed;
    public Vector3 dir;
    public int dam;
    public float lifetime;
    public bool returnbullet;
    public bool overrideMovement;

    ObjectPooler objectPool;

    List<BezierCurve> bezierCurves;
    public float bezierPower = -.025f;
    float currentProgress;

    public delegate void Hit(int value);
    public static event Hit OnHit;

    [SerializeField] private LayerMask ignoreBullet;

    [Header("Feedbacks")]
    public MMFeedbacks DamageFeedback;

    /// <summary>
    /// This function is called when the object pooling system recycles this object
    /// The function will reset all bullet values and variables to default
    /// </summary>
    public virtual void OnObjectSpawn()
    {
        if(!rb)
        {
            rb = GetComponent<Rigidbody>();
        }
        
        Debug.Log(dir);
        rb.velocity = dir * speed * Time.fixedDeltaTime;

        StartCoroutine(DieTime());
    }

    protected virtual void Start()
    {
        objectPool = ObjectPooler.Instance;
    }

    protected List<BezierCurve> GenerateCurves()
    {
        List<BezierCurve> tempCurves = new List<BezierCurve> { };

        Vector2 pos = new Vector2(transform.position.x, transform.position.z);

        tempCurves.Add(new BezierCurve(pos, new Vector2(pos.x + (50 * bezierPower), pos.y + (100 * bezierPower)), new Vector2(pos.x + (100 * bezierPower), pos.y + (100 * bezierPower)), new Vector2(pos.x + (150 * bezierPower), pos.y + (75 * bezierPower))));

        pos = new Vector2(pos.x + (150 * bezierPower), pos.y + (75 * bezierPower));

        for (int i = 0; i < 6; i++)
        {
            tempCurves.Add(new BezierCurve(pos, new Vector2(pos.x - (100 * bezierPower), pos.y + (150 * bezierPower)), new Vector2(pos.x - (150 * bezierPower), pos.y + (150 * bezierPower)), new Vector2(pos.x - (250 * bezierPower), pos.y + (100 * bezierPower))));
            pos = new Vector2(pos.x - (250 * bezierPower), pos.y + (100 * bezierPower));
            tempCurves.Add(new BezierCurve(pos, new Vector2(pos.x + (100 * bezierPower), pos.y + (150 * bezierPower)), new Vector2(pos.x + (150 * bezierPower), pos.y + (150 * bezierPower)), new Vector2(pos.x + (259 * bezierPower), pos.y + (100 * bezierPower))));
            pos = new Vector2(pos.x + (250 * bezierPower), pos.y + (100 * bezierPower));
        }

        return tempCurves;
    }

    protected virtual void Update()
    {
        print(ignoreBullet.value);

        /* BEZIER CURVE CODE IF WANTED/NEEDED LATER IN PROJECT
        if (isSnowfall && !overrideMovement)
        {
            if(currentProgress > snowfallCurve.Count - 1)
            {
                gameObject.SetActive(false);
            }

            int curCurve = Mathf.FloorToInt(currentProgress); float posOnCurve = currentProgress % 1;

            // Play sound right before starting new curve
            if (posOnCurve > 0.97f)
            {
                RuntimeManager.PlayOneShot(eSnowfall);
            }

            //Debug.Log(curCurve + " " + posOnCurve);
            Vector2 drawPos = snowfallCurve[curCurve].ReturnCurve(posOnCurve);

            Vector3 moveTo = new Vector3(drawPos.x, transform.position.y, drawPos.y);
            transform.position = Vector3.Lerp(transform.position, moveTo, .1f);

            currentProgress += .0001f * speed * Time.deltaTime * 75;
        }
        */
    }

    // Basic checks to see if the player should take damage or not
    public virtual void OnTriggerEnter(Collider other)
    {
        if (!other.GetComponent<BulletMovement>() && !other.GetComponent<Enemy>())
        {
            if (other.CompareTag("Player"))
            {
                //other.GetComponent<Health>().Damage(dam);

                OnHit?.Invoke(dam);
                DamageFeedback?.PlayFeedbacks();
            }


            if (other.gameObject.layer == ignoreBullet)
            {
                gameObject.SetActive(false);
            }

        }
    }

    protected virtual IEnumerator DieTime()
    {
        // Lifetime for the bullet
        yield return new WaitForSeconds(lifetime);
        if (returnbullet)
        {
            rb.velocity = -rb.velocity;
            returnbullet = false;
            StartCoroutine(DieTime());
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}