using UnityEngine;

namespace IntegratedGameplaySystem
{
    public class MouseMovement 
    {
        private const float MIN_CAM_ANGLE = -90f;
        private const float MAX_CAM_ANGLE = 90f;

        private readonly Sensitivity sensitivity;
        private readonly Transform eyes;
        private readonly Transform transform;
        private Vector2 lookingDirection;

        public MouseMovement(Transform eyes, Transform transform, Sensitivity sensitivity)
        {
            this.eyes = eyes;
            this.sensitivity = sensitivity;
            this.transform = transform;
        }

        public void Update(Vector2 mouseDirectionChange)
        {
            lookingDirection += mouseDirectionChange * sensitivity.Value;
            lookingDirection.y = Mathf.Clamp(lookingDirection.y, MIN_CAM_ANGLE, MAX_CAM_ANGLE);

            eyes.localRotation = Quaternion.AngleAxis(-lookingDirection.y, Vector3.right);
            transform.localRotation = Quaternion.AngleAxis(lookingDirection.x, Vector3.up);
        }
    }
}