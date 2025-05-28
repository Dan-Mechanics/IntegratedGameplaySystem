using UnityEngine;
using System.Collections.Generic;

namespace IntegratedGameplaySystem
{
    [CreateAssetMenu(menuName = "ScriptableObjects/" + nameof(MovementSettings), fileName = "New " + nameof(MovementSettings))]
    public class MovementSettings : ScriptableObject
    {
        public float walkSpeed;
        public float runSpeed;
        public float flySpeed;
        public float movAccel;
        public float airborneAccelMult;
    }
}