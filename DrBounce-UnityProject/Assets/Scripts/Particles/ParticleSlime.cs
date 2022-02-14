using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSlime : MonoBehaviour
{
    private DecalManager decalM;
    private int colour;

    private void Start()
    {
        decalM = DecalManager.Instance;


        //this is actually cringe please never talk to me or my family again
        Color particleSysColour = GetComponent<ParticleSystemRenderer>().material.GetColor("_Color");
        string colourHEX = ColorUtility.ToHtmlStringRGB(particleSysColour);

        switch (colourHEX) {
            case "9A3FEF":
                colour = 0;
                break;
            case "0099FF":
                colour = 1;
                break;
            case "FF0000":
                colour = 2;
                break;
            case "EFF300":
                colour = 3;
                break;
        }
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

            decalM.SpawnDecalWithColour(pos, normal, 1f, collisionEvents[i].colliderComponent.transform, DecalManager.DecalType.slime, colour);
        }
    }
}
