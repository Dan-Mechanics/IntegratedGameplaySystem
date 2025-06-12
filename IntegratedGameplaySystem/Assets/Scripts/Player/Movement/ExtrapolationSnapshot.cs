using UnityEngine;

namespace IntegratedGameplaySystem
{
    public struct ExtrapolationSnapshot
    {
        public Vector3 pos;
        public Vector3 vel;
        public float time;

        public void Set(Vector3 pos, Vector3 vel, float time)
        {
            this.pos = pos;
            this.vel = vel;
            this.time = time;
        }

        public Vector3 ExtrapolatePosition()
        {
            return pos + (vel * (Time.time - time));
        }
    }
}