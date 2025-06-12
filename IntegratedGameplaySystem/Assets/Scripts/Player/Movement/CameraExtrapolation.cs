using UnityEngine;

namespace IntegratedGameplaySystem
{
    public class CameraExtrapolation
    {
        private readonly Transform camera;
        private ExtrapolationSnapshot snapshot;

        public CameraExtrapolation(Transform camera)
        {
            this.camera = camera;
        }

        public void Update() => camera.position = snapshot.ExtrapolatePosition();
        public void UpdateCameraRotation(Quaternion rot) => camera.rotation = rot;
        public void SetSnapshot(ExtrapolationSnapshot snapshot) => this.snapshot = snapshot;
    }
}