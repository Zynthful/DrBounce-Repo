using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSlime : MonoBehaviour
{
    private DecalManager decalM;
    [SerializeField] Material decal = null;

    private void Start()
    {
        decalM = DecalManager.Instance;
    }

    void OnParticleCollision(GameObject other)
    {
        Debug.Log(other.name);

        List<ParticleCollisionEvent> collisionEvents = new List<ParticleCollisionEvent>();

        ParticlePhysicsExtensions.GetCollisionEvents(this.GetComponent<ParticleSystem>(), other, collisionEvents);

        for (int i = 0; i < collisionEvents.Count; i++)
        {
            Vector3 pos = collisionEvents[i].intersection;
            Vector3 normal = collisionEvents[i].normal;

            decalM.SpawnDecal(pos, normal, 1f, DecalManager.DecalType.placeholder);
        }
    }
}
