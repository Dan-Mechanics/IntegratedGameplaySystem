using UnityEngine;

namespace IntegratedGameplaySystem
{
    public class CameraHandler
    {
        private readonly Transform camera;
        private Tick tick;

        public CameraHandler(Transform camera)
        {
            this.camera = camera;
        }

        public void Update() => camera.position = tick.CalculatePos();
        public void UpdateRot(Quaternion rot) => camera.rotation = rot;
        public void SetTick(Tick tick) => this.tick = tick;

        public struct Tick
        {
            public Vector3 pos;
            public Vector3 vel;
            public float time;

            public Tick(Vector3 pos, Vector3 vel, float time)
            {
                this.pos = pos;
                this.vel = vel;
                this.time = time;
            }

            public void Set(Vector3 pos, Vector3 vel, float time)
            {
                this.pos = pos;
                this.vel = vel;
                this.time = time;
            }

            public Vector3 CalculatePos() 
            {
                return pos + (vel * (Time.time - time));
            }
        }
    }
}