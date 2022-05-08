using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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

    public LayerMask[] layersToIgnore = null;

    [Header("Base Bullet Events")]
    public UnityEvent onSpawn = null;
    public UnityEvent onDeath = null;
    public UnityEvent<GameObject> onHitAny = null;
    public UnityEvent<GameObject> onHitPlayer = null;
    public UnityEvent<GameObject> onHitAnyExceptPlayer = null;

    public delegate void Hit(int value, bool ignoreGod = false);
    public static event Hit OnHit;

    /// <summary>
    /// This function is called when the object pooling system recycles this object
    /// The function will reset all bullet values and variables to default
    /// </summary>
    public virtual void OnObjectSpawn()
    {
        if(!rb)
        {
            rb = GetComponent<Rigidbody>();
            if (!rb)
                rb = GetComponentInChildren<Rigidbody>();
        }

        rb.constraints = RigidbodyConstraints.None;
        rb.velocity = dir * speed * Time.fixedDeltaTime;

        transform.LookAt(transform.position + dir);

        onSpawn.Invoke();

        StartCoroutine(DieTime());
    }

    protected virtual void Start()
    {
        objectPool = ObjectPooler.Instance;
    }

    protected virtual void OnDisable()
    {
        onDeath.Invoke();
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
        //print(ignoreBullet.value);

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

    // Check for what we've hit (ignored layer, non-ignored layer/player)
    public virtual void OnTriggerEnter(Collider other)
    {
        if (!other.GetComponent<BulletMovement>() && !other.GetComponent<Enemy>())
        {
            // Check for any ignored layers, and if we haven't found one, we've hit something not ignored
            bool foundIgnoredLayer = false;
            for (int i = 0; i < layersToIgnore.Length; i++)
            {
                // Check if colliding object's layer matches ignored layer (h e l p)
                if ((layersToIgnore[i].value & (1 << other.gameObject.layer)) > 0)
                {
                    foundIgnoredLayer = true;
                    break;
                }
            }
            if (!foundIgnoredLayer)
            {
                if (other.CompareTag("Player"))
                {
                    //other.GetComponent<Health>().Damage(dam);

                    onHitPlayer?.Invoke(other.gameObject);
                    onHitAny?.Invoke(other.gameObject);
                    OnHit?.Invoke(dam);
                }
                else
                {
                    onHitAny?.Invoke(other.gameObject);
                    onHitAnyExceptPlayer?.Invoke(other.gameObject);
                }
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