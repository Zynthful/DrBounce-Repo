using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SamDriver.Decal;

public class DecalManager : MonoBehaviour
{
    [SerializeField]
    private DecalAsset decalToSpawn = null;

    private List<DecalMesh> spawnedDecals = new List<DecalMesh>();

    // Start is called before the first frame update
    void Start()
    {

    }

    public void SpawnDecal(Vector3 t, MeshFilter hitMesh, Vector3 normal, float decalScale)
    {
        spawnedDecals.Add(CreateDecal(t, hitMesh, normal, decalScale, decalToSpawn));
        
        foreach (var decal in spawnedDecals)
        {
            if (decal.HasMeshToProjectAgainst)
            {
                Debug.Log(decal.HasMeshToProjectAgainst);

                // because we're spawning several decals at once it's possible to take more than
                // 4 frames for all the jobs to complete.
                // (this changes how the temporary memory used by the jobs is allocated)
                bool mayTakeMoreThanFourFrames = true;

                // begin to perform the projection
                // if you want the decal mesh to be generated immediately use GenerateProjectedMeshImmediate,
                // but beware it can easily lock up the main thread and cause skipped frames.
                //decal.GenerateProjectedMeshDelayed(mayTakeMoreThanFourFrames);
                decal.GenerateProjectedMeshImmediate();
            }
        }
        
    }

    private DecalMesh CreateDecal(Vector3 localPos, MeshFilter hitMesh, Vector3 normal, float decalScale, DecalAsset decalToSpawn)
    {
        // create game object as child of the spawner
        GameObject decalObject = new GameObject($"Spawned Decal ({localPos.x.ToString("F3")},{localPos.y.ToString("F3")})");
        decalObject.transform.SetParent(this.transform, false);

        // set the transform however you wish, keeping in mind that rotation affects how it'll be projected
        decalObject.transform.localPosition = localPos;
        decalObject.transform.localScale = Vector3.one * decalScale;
        decalObject.transform.rotation = Quaternion.FromToRotation(Vector3.back, normal);

        // needs a MeshFilter and MeshRenderer to render the decal
        decalObject.AddComponent<MeshFilter>();
        var meshRenderer = decalObject.AddComponent<MeshRenderer>();

        // decals generally shouldn't cast shadows
        meshRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;

        // create the DecalMesh component itself
        var decal = decalObject.AddComponent<DecalMesh>();

        // provide it with a DecalAsset
        decal.DecalAsset = decalToSpawn;

        // scale the decal's object to match selected decal's ratio
        // (same behaviour as clicking the "Scale to match decal shape" button)
        decal.ScaleToMatchDecalBoundsRatio();

        // we can set any of the options available on the DecalMesh component,
        // here we'll horizontally flip some of them at random
        decal.IsFlipU = (Random.value > 0.5f);

        // SetupMaterialPropertyBlock should be called whenever the DecalAsset or
        // the per-decal options like flip and opacity are changed.
        decal.SetupMaterialPropertyBlock();

        // set up what the decal will project against
        // if you skip this it'll default to projecting against any nearby static meshes
        decal.ShouldUseSceneStaticMeshes = false;
        List<MeshFilter> meshForProjection = new List<MeshFilter>();
        meshForProjection.Add(hitMesh);
        decal.MeshesToProjectAgainst = meshForProjection;

        return decal;
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
