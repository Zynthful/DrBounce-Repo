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

        //rotate around the z axis randomly
        decalRotation *= Quaternion.Euler(Vector3.forward * Random.Range(0, 180));

        //place object from pool
        GameObject decalObject = pool.SpawnNonBulletFromPool("Decal", position + (decalRotation * Vector3.back * (0.01f + Random.Range(0f, 0.02f))), decalRotation, decalToSpawn);
        //$"Spawned Decal ({localPos.x.ToString("F3")},{localPos.y.ToString("F3")})"

        //randomly adjust decal scale for fun :)
        decalScale *= Random.Range(0.9f, 1.1f);

        //scale that bad boy and flip randomly
        decalObject.transform.localScale = new Vector3(((Random.Range(0, 2) * 2) - 1) * decalScale, decalScale, decalScale);

        //set parent to manager or supplied transform
        decalObject.transform.SetParent(t, true);

        return decalObject;
    }

    public void ClearDecals()
    {
        //whatever
        foreach (var decal in spawnedDecals)
        {
            decal.SetActive(false);
        }
        spawnedDecals.Clear();
    }
}
