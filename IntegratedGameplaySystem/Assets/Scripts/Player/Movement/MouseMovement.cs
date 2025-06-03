using UnityEngine;

namespace IntegratedGameplaySystem
{
    /// <summary>
    /// NEED: 
    /// sens data
    /// cam
    /// transform
    /// IPlayerInput basically.
    /// 
    /// and then we just update and done.
    /// </summary>
    public class MouseMovement 
    {
        private const float MIN_CAM_ANGLE = -90f;
        private const float MAX_CAM_ANGLE = 90f;

        private readonly float sens;
        private readonly Transform eyes;
        private readonly Transform transform;
        private Vector2 lookingDirection;

        public MouseMovement(Transform eyes, Transform transform, float sens)
        {
            this.eyes = eyes;
            this.sens = sens;
            this.transform = transform;
        }

        /// <summary>
        /// Use PlayerInput. VV
        /// </summary>
        public void Update(Vector2 mouseDirectionChange)
        {
            lookingDirection += mouseDirectionChange * sens;
            lookingDirection.y = Mathf.Clamp(lookingDirection.y, MIN_CAM_ANGLE, MAX_CAM_ANGLE);

            eyes.localRotation = Quaternion.AngleAxis(-lookingDirection.y, Vector3.right);
            transform.localRotation = Quaternion.AngleAxis(lookingDirection.x, Vector3.up);
        }
    }
}