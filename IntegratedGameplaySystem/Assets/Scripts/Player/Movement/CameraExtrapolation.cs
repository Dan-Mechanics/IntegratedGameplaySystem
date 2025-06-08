using UnityEngine;

namespace IntegratedGameplaySystem
{
    public class CameraExtrapolation : IUpdatable
    {
        private readonly Transform camera;
        private CameraMotionSnapshot snapshot;

        public CameraExtrapolation(Transform camera)
        {
            this.camera = camera;
        }

        public void Update() => camera.position = snapshot.CalculateCameraPosition();
        public void UpdateRot(Quaternion rot) => camera.rotation = rot;
        public void SetSnapshot(CameraMotionSnapshot snapshot) => this.snapshot = snapshot;
    }
}