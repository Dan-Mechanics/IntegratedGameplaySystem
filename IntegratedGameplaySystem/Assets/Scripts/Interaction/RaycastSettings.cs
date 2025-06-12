using UnityEngine;
using System.Collections.Generic;

namespace IntegratedGameplaySystem
{
    [CreateAssetMenu(menuName = "ScriptableObjects/" + nameof(RaycastSettings), fileName = "New " + nameof(RaycastSettings))]
    public class RaycastSettings : ScriptableObject
    {
        public float range;
        public LayerMask mask;
        public QueryTriggerInteraction interaction;
        public List<string> tags;
    }
}