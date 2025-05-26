using System;
using System.Collections.Generic;
using UnityEngine;

namespace IntegratedGameplaySystem
{
    /// <summary>
    /// Basically does raycasts. Make component instead??
    /// </summary>
    public class Raycaster
    {
        private readonly RaycastData data;

        public Raycaster(RaycastData data)
        {
            this.data = data;
        }

        public Transform Raycast(Vector3 pos, Vector3 dir) 
        {
            if (!Physics.Raycast(pos, dir, out RaycastHit hit, data.range, data.mask, data.triggerInteraction))
                return null;

            if (!data.tags.Contains(hit.transform.tag))
                return null;

            return hit.transform;
        }

        [Serializable]
        public struct RaycastData 
        {
            public float range;
            public LayerMask mask;
            public QueryTriggerInteraction triggerInteraction;
            public List<string> tags;
        }
    }
}