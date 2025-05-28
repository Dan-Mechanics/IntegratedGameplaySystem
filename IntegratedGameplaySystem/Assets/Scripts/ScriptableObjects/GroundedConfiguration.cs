using UnityEngine;
using System.Collections.Generic;

namespace IntegratedGameplaySystem
{
    [CreateAssetMenu(menuName = "ScriptableObjects/" + nameof(GroundedConfiguration), fileName = "New " + nameof(GroundedConfiguration))]
    public class GroundedConfiguration : ScriptableObject
    {
        public LayerMask mask;
        public float radius;
        public float reach;
        public float maxAngle;
    }
}