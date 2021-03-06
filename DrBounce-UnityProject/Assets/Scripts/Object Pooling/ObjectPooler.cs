using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ObjectPooler : MonoBehaviour
{
    [System.Serializable]
    public class Pool
    {
        public string tag;
        public GameObject prefab;
        public int size;
    }

    #region Singleton

    public static ObjectPooler Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = FindObjectOfType(typeof(ObjectPooler)) as ObjectPooler;
        }

        if (Instance == null)
        {
            Instance = this;
        }
    }

    #endregion

    public List<Pool> pools;
    public Dictionary<string, Queue<GameObject>> poolDictionary;

    [SerializeField] Mesh defaultBulletMesh;

    void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;

        poolDictionary = new Dictionary<string, Queue<GameObject>>();

        foreach(Pool pool in pools)
        {
            Queue<GameObject> objPool = new Queue<GameObject>();

            for (int i=0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab);
                obj.transform.parent = transform;
                obj.SetActive(false);
                objPool.Enqueue(obj);
            }

            poolDictionary.Add(pool.tag, objPool);
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        foreach (Queue<GameObject> pool in poolDictionary.Values)
        {
            foreach(GameObject obj in pool)
            {
                if(obj != null)
                {
                    obj.transform.parent = transform;
                    obj.SetActive(false);
                }
            }
        }
    }

    public GameObject SpawnBulletFromPool(string tag, Vector3 position, Quaternion rotation, Vector3 dir, BulletType bul, Material mat)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            //Debug.LogWarning("Pool with tag " + tag + " doesn't exist!");
            return null;
        }

        GameObject objToSpawn = poolDictionary[tag].Dequeue();

        objToSpawn.SetActive(true);

        bul.typeSwitch.SetValue(objToSpawn);

        if (mat != null)
        {
            objToSpawn.GetComponent<Renderer>().material = mat;
        }

        BulletMovement objMov;

        if (objToSpawn.GetComponent<BulletMovement>())
        {
            objMov = objToSpawn.GetComponent<BulletMovement>();
        }
        else
        {
            objMov = objToSpawn.GetComponentInChildren<BulletMovement>();
        }

        objMov.returnbullet = false;
        objMov.dir = dir;
        objMov.speed = bul.speed;
        objMov.dam = bul.damage;
        objMov.lifetime = bul.lifespan;
        objToSpawn.transform.localScale = bul.size;
        objToSpawn.transform.position = position;

        IPooledObject pooledObj = objMov.GetComponent<IPooledObject>();

        if (pooledObj != null)
        {
            pooledObj.OnObjectSpawn();
        }

        poolDictionary[tag].Enqueue(objToSpawn);

        return objToSpawn;
    }

    public GameObject SpawnNonBulletFromPool(string tag, Vector3 position, Quaternion rotation, Material mat)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            //Debug.LogWarning("Pool with tag " + tag + " doesn't exist!");
            return null;
        }

        GameObject objToSpawn = poolDictionary[tag].Dequeue();

        objToSpawn.SetActive(true);

        if(mat != null)
        {
            objToSpawn.GetComponent<Renderer>().material = mat;
        }

        objToSpawn.transform.position = position;
        objToSpawn.transform.rotation = rotation;

        IPooledObject pooledObj = objToSpawn.GetComponent<IPooledObject>();

        if (pooledObj != null)
        {
            pooledObj.OnObjectSpawn();
        }

        poolDictionary[tag].Enqueue(objToSpawn);

        return objToSpawn;
    }
}