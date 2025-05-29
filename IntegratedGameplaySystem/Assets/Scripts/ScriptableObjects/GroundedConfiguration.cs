using UnityEngine;
using System.Collections.Generic;

namespace IntegratedGameplaySystem
{
    /// <summary>
    /// Consider making this one with movement settings and then adding eyes height for the sport.
    /// call it basicplayer settings but it need sto be different from interactio raycast shoit.
    /// </summary>
    [CreateAssetMenu(menuName = "ScriptableObjects/" + nameof(GroundedConfiguration), fileName = "New " + nameof(GroundedConfiguration))]
    public class GroundedConfiguration : ScriptableObject
    {
        public LayerMask mask;
        public float radius;
        public float reach;
        public float maxAngle;
    }
}