using UnityEngine;

namespace IntegratedGameplaySystem
{
    public class MouseMovement 
    {
        private Transform eyes;
        private Transform trans;

        private const float MIN_CAM_ANGLE = -90f;
        private const float MAX_CAM_ANGLE = 90f;

        private float sensitvity = 0.33f;
        private Vector2 mouseDirection;
        /*private float mouseX;
        private float mouseY;*/

        public MouseMovement(Transform eyes, Transform trans)
        {
            this.eyes = eyes;
            this.trans = trans;
        }

        /// <summary>
        /// Use PlayerInput.
        /// </summary>
        public void Update(Vector2 mouseDirectionChange)
        {
            /*mouseX = Input.GetAxisRaw("Mouse X");
            mouseY = Input.GetAxisRaw("Mouse Y");

            Vector2 mouseDirectionChange = new Vector2(mouseX, mouseY);*/
            mouseDirection += mouseDirectionChange * sensitvity;
            mouseDirection.y = Mathf.Clamp(mouseDirection.y, MIN_CAM_ANGLE, MAX_CAM_ANGLE);

            eyes.localRotation = Quaternion.AngleAxis(-mouseDirection.y, Vector3.right);
            trans.localRotation = Quaternion.AngleAxis(mouseDirection.x, Vector3.up);
        }
    }
}