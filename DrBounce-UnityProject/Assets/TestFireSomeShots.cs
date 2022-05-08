using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestFireSomeShots : MonoBehaviour
{

    [SerializeField] GameObject[] spawners;

    [HideInInspector] public bool inFlight;

    private float shotDelay;
    private bool switchSpawn;

    [SerializeField] private BulletType shotType;

    ObjectPooler pooler;

    // Start is called before the first frame update
    void Start()
    {
        pooler = ObjectPooler.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        if(inFlight && CanShoot())
        {
            if(switchSpawn)
                pooler.SpawnBulletFromPool("ExplosiveShot", spawners[0].transform.position + (Vector3.down * 2), Quaternion.LookRotation(transform.TransformDirection(Vector3.down)), Vector3.down, shotType, null);
            else
                pooler.SpawnBulletFromPool("ExplosiveShot", spawners[1].transform.position + (Vector3.down * 2), Quaternion.LookRotation(transform.TransformDirection(Vector3.down)), Vector3.down, shotType, null);
            shotDelay = 0.55f;
        }

        if(shotDelay > 0)
            shotDelay -= Time.deltaTime;
    }

    bool CanShoot(){return shotDelay <= 0;}
}
