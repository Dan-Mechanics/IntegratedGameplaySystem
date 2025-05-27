using UnityEngine;

namespace IntegratedGameplaySystem
{
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
    }
}