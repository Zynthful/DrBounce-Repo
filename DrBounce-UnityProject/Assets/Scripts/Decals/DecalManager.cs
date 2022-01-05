using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecalManager : MonoBehaviour
{
    #region Singleton

    public static DecalManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = FindObjectOfType(typeof(DecalManager)) as DecalManager;
        }

        if (Instance == null)
        {
            Instance = this;
        }
    }

    #endregion

    [SerializeField]
    private Material defaultDecal = null;

    private List<GameObject> spawnedDecals = new List<GameObject>();

    private ObjectPooler pool;

    // Start is called before the first frame update
    void Start()
    {
        pool = ObjectPooler.Instance;
    }

    public void SpawnDecal(Vector3 position, Vector3 normal, float decalScale)
    {
        spawnedDecals.Add(CreateDecal(position, normal, decalScale, this.transform, defaultDecal));
    }

    public void SpawnDecal(Vector3 position, Vector3 normal, float decalScale, Material decalMaterial)
    {
        spawnedDecals.Add(CreateDecal(position, normal, decalScale, this.transform, decalMaterial));
    }

    public void SpawnDecal(Vector3 position, Vector3 normal, float decalScale, Transform t, Material decalMaterial)
    {
        spawnedDecals.Add(CreateDecal(position, normal, decalScale, t, decalMaterial));
    }

    private GameObject CreateDecal(Vector3 position, Vector3 normal, float decalScale, Transform t, Material decalToSpawn)
    {
        if (decalToSpawn == null)
        {
            decalToSpawn = defaultDecal;
        }

        Quaternion decalRotation = Quaternion.FromToRotation(Vector3.back, normal);
        decalRotation *= Quaternion.Euler(Vector3.forward * Random.Range(0, 180));

        // create game object as child of the spawner
        GameObject decalObject = pool.SpawnNonBulletFromPool("Decal", position + (decalRotation * Vector3.back * 0.01f), decalRotation, decalToSpawn);
        //$"Spawned Decal ({localPos.x.ToString("F3")},{localPos.y.ToString("F3")})"
        decalObject.transform.localScale = Vector3.one * decalScale;

        Debug.Log(t);
        decalObject.transform.SetParent(t, true);

        return decalObject;
    }

    public void ClearDecals()
    {
        foreach (var decal in spawnedDecals)
        {
            Destroy(decal.gameObject);
        }
        spawnedDecals.Clear();
    }
}
