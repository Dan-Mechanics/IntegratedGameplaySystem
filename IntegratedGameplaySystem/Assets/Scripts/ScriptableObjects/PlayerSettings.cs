using UnityEngine;
using System.Collections.Generic;

namespace IntegratedGameplaySystem
{
    [CreateAssetMenu(menuName = "ScriptableObjects/" + nameof(PlayerSettings), fileName = "New " + nameof(PlayerSettings))]
    public class PlayerSettings : ScriptableObject
    {
        [Header("Movement")]
        public float walkSpeed;
        public float runSpeed;
        public float flySpeed;
        public float movAccel;
        public float airborneAccelMult;

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