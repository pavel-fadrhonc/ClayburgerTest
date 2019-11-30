using System.Collections.Generic;
using UnityEngine;

namespace Utils
{
    public class PlayParticle : MonoBehaviour
    {
        public List<ParticleSystem> ParticleSystem;

        public void PlayParticleSystems()
        {
            ParticleSystem.ForEach(ps => ps.Play());
        }
    }
}