using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[RequireComponent(typeof(ParticleSystem))]
public class InstantiateObjectOnParticleSpawn : MonoBehaviour
{
    private ParticleSystem system;

    [SerializeField]
    private GameObject instantiatedObject = null;

    private IDictionary<uint, ParticleSystem.Particle> trackedParticles = new Dictionary<uint, ParticleSystem.Particle>();

    private void Awake()
    {
        system = GetComponent<ParticleSystem>();
    }

    private void Update()
    {
        var activeParticles = new ParticleSystem.Particle[system.particleCount];
        system.GetParticles(activeParticles);

        var particleDelta = GetParticleDelta(activeParticles);

        foreach (var particleAdded in particleDelta.Added)
        {
            GameObject obj = Instantiate(instantiatedObject, transform);
            obj.transform.localPosition = particleAdded.position;
        }
    }

    private ParticleDelta GetParticleDelta(ParticleSystem.Particle[] liveParticles)
    {
        var deltaResult = new ParticleDelta();

        foreach (var activeParticle in liveParticles)
        {
            ParticleSystem.Particle foundParticle;
            if (trackedParticles.TryGetValue(activeParticle.randomSeed, out foundParticle))
            {
                trackedParticles[activeParticle.randomSeed] = activeParticle;
            }
            else
            {
                deltaResult.Added.Add(activeParticle);
                trackedParticles.Add(activeParticle.randomSeed, activeParticle);
            }
        }

        var updatedParticleAsDictionary = liveParticles.ToDictionary(x => x.randomSeed, x => x);
        var dictionaryKeysAsList = trackedParticles.Keys.ToList();

        foreach (var dictionaryKey in dictionaryKeysAsList)
        {
            if (updatedParticleAsDictionary.ContainsKey(dictionaryKey) == false)
            {
                deltaResult.Removed.Add(trackedParticles[dictionaryKey]);
                trackedParticles.Remove(dictionaryKey);
            }
        }

        return deltaResult;
    }

    private class ParticleDelta
    {
        public IList<ParticleSystem.Particle> Added { get; set; } = new List<ParticleSystem.Particle>();
        public IList<ParticleSystem.Particle> Removed { get; set; } = new List<ParticleSystem.Particle>();
    }
}