using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSlime : MonoBehaviour
{
    private DecalManager decalM;

    private void Start()
    {
        decalM = DecalManager.Instance;
    }

    void OnParticleCollision(GameObject other)
    {
        List<ParticleCollisionEvent> collisionEvents = new List<ParticleCollisionEvent>();

        ParticlePhysicsExtensions.GetCollisionEvents(this.GetComponent<ParticleSystem>(), other, collisionEvents);

        for (int i = 0; i < collisionEvents.Count; i++)
        {
            //Debug.Log((collisionEvents[i].colliderComponent.gameObject.name) + ", new particle spawn :)");

            Vector3 pos = collisionEvents[i].intersection;
            Vector3 normal = collisionEvents[i].normal;

            decalM.SpawnDecal(pos, normal, 1f, collisionEvents[i].colliderComponent.transform, DecalManager.DecalType.slime);
        }
    }
}
