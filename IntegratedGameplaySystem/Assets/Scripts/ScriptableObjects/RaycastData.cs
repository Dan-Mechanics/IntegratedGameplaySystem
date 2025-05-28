using UnityEngine;
using System.Collections.Generic;

namespace IntegratedGameplaySystem
{
    [CreateAssetMenu(menuName = "ScriptableObjects/" + nameof(RaycastData), fileName = "New " + nameof(RaycastData))]
    public class RaycastData : ScriptableObject
    {
        public float range;
        public LayerMask mask;
        public QueryTriggerInteraction triggerInteraction;
        public List<string> tags;
    }
}