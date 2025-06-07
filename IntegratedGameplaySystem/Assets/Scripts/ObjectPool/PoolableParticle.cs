using UnityEngine;

namespace IntegratedGameplaySystem
{
    public class PoolableParticle : IPoolable
    {
        private readonly ParticleSystem particle;

        public PoolableParticle(ParticleSystem particle)
        {
            this.particle = particle;
        }

        public void Place(Vector3 pos) 
        {
            particle.transform.position = pos;
        }

        public void Disable()
        {
            particle.Stop();
        }

        public void Enable()
        {
            particle.Play();
        }

        public void Flush()
        {
            if (particle == null)
                return;

            Disable();
            Object.Destroy(particle.gameObject);
        }
    }
}
