using System.Collections.Generic;
using UnityEngine;

namespace IntegratedGameplaySystem
{
    /// <summary>
    /// Basically does raycasts. Make component instead??
    /// </summary>
    public class Raycaster
    {
        private readonly float range;
        private readonly LayerMask mask;
        private readonly QueryTriggerInteraction triggerInteraction;
        private readonly List<string> tags;

        public Raycaster(float range, LayerMask mask, QueryTriggerInteraction triggerInteraction = QueryTriggerInteraction.Ignore, List<string> tags = null)
        {
            if (range <= 0f)
                range = 1f;
            
            this.range = range;
            this.mask = mask;
            this.triggerInteraction = triggerInteraction;
            this.tags = tags ?? new List<string>() { "Default" };
        }

        public Transform Raycast(Vector3 pos, Vector3 dir) 
        {
            if (!Physics.Raycast(pos, dir, out RaycastHit hit, range, mask, triggerInteraction))
                return null;

            if (!tags.Contains(hit.transform.tag))
                return null;

            return hit.transform;
        }
    }
}