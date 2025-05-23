using UnityEngine;

namespace IntegratedGameplaySystem
{
    public class MouseMovement 
    {
        private const float MIN_CAM_ANGLE = -90f;
        private const float MAX_CAM_ANGLE = 90f;
        private const float SENS = 0.33f;

        private readonly Transform eyes;
        private readonly Transform trans;
        private Vector2 mouseDir;

        public MouseMovement(Transform eyes, Transform trans)
        {
            this.eyes = eyes;
            this.trans = trans;
        }

        /// <summary>
        /// Use PlayerInput. VV
        /// </summary>
        public void Update(Vector2 mouseDirectionChange)
        {
            mouseDir += mouseDirectionChange * SENS;
            mouseDir.y = Mathf.Clamp(mouseDir.y, MIN_CAM_ANGLE, MAX_CAM_ANGLE);

            eyes.localRotation = Quaternion.AngleAxis(-mouseDir.y, Vector3.right);
            trans.localRotation = Quaternion.AngleAxis(mouseDir.x, Vector3.up);
        }
    }
}