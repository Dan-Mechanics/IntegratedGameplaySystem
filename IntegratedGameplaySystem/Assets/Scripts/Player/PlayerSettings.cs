using UnityEngine;

namespace IntegratedGameplaySystem
{
    [CreateAssetMenu(menuName = "ScriptableObjects/" + nameof(PlayerSettings), fileName = "New " + nameof(PlayerSettings))]
    public class PlayerSettings : ScriptableObject
    {
        public GameObject prefab;

        [Header("Movement")]
        public float walkSpeed;
        public float runSpeed;
        public float flySpeed;
        public float movAccel;
        public float accelMult;

        [Header("Grounded")]
        public LayerMask mask;
        public float radius;
        public float reach;
        public float maxAngle;

        [Header("Camera")]
        public float sens;
        public float eyesHeight;
    }
}