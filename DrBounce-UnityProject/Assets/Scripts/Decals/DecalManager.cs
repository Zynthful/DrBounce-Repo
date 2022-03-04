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

    public enum DecalType
    {
        bullet,
        slime,
        placeholder
    }

    [SerializeField]
    private Material placeholderDecal = null;

    [SerializeField]
    private Material bulletHoleDecal = null;
    
    [SerializeField]
    private Material[] slimeDecals = new Material[4];

    private List<GameObject> spawnedDecals = new List<GameObject>();

    private ObjectPooler pool;

    [Header("AkEvents")]
    [SerializeField]
    private AK.Wwise.Event slimeEvent = null;
    [SerializeField]
    private AK.Wwise.Event bulletHoleEvent = null;
    [SerializeField]
    private AK.Wwise.Event placeHolderEvent = null;

    void Start()
    {
        pool = ObjectPooler.Instance;
    }

    public void SpawnDecal(Vector3 position, Vector3 normal, float decalScale)
    {
        spawnedDecals.Add(CreateDecal(position, normal, decalScale, this.transform, DecalType.placeholder));
    }
    public void SpawnDecal(Vector3 position, Vector3 normal, float decalScale, DecalType decalType)
    {
        spawnedDecals.Add(CreateDecal(position, normal, decalScale, this.transform, decalType));
    }

    public void SpawnDecal(Vector3 position, Vector3 normal, float decalScale, Transform t, DecalType decalType)
    {
        spawnedDecals.Add(CreateDecal(position, normal, decalScale, t, decalType));
    }
    
    public void SpawnDecalWithColour(Vector3 position, Vector3 normal, float decalScale, Transform t, DecalType decalType, int colour)
    {
        spawnedDecals.Add(CreateDecal(position, normal, decalScale, t, decalType, colour));
    }

    private GameObject CreateDecal(Vector3 position, Vector3 normal, float decalScale, Transform t, DecalType decalType, int? colour = 0)
    {
        Material decalToSpawn;

        AK.Wwise.Event sound;

        switch (decalType)
        {
            case DecalType.bullet:
                decalToSpawn = bulletHoleDecal;
                sound = bulletHoleEvent;
                break;
            case DecalType.slime:
                decalToSpawn = slimeDecals[(int)colour];
                sound = slimeEvent;
                break;
            default:
                decalToSpawn = placeholderDecal;
                sound = placeHolderEvent;
                break;
        }

        Quaternion decalRotation = Quaternion.FromToRotation(Vector3.back, normal);

        //rotate around the z axis randomly
        decalRotation *= Quaternion.Euler(Vector3.forward * Random.Range(0, 180));

        //place object from pool
        GameObject decalObject = pool.SpawnNonBulletFromPool("Decal", position + (decalRotation * Vector3.back * (0.01f + Random.Range(0f, 0.02f))), decalRotation, decalToSpawn);

        //randomly adjust decal scale for fun :)
        decalScale *= Random.Range(0.9f, 1.1f);

        //scale that bad boy and flip randomly
        decalObject.transform.localScale = new Vector3(((Random.Range(0, 2) * 2) - 1) * decalScale, decalScale, decalScale);

        //set parent to manager or supplied transform
        decalObject.transform.SetParent(t, true);

        sound.Post(decalObject);

        return decalObject;
    }

    public void ClearDecals()
    {
        //whatever
        foreach (var decal in spawnedDecals)
        {
            if(decal != null)
                decal.SetActive(false);
        }
        spawnedDecals.Clear();
    }
}
