using UnityEngine;

namespace IntegratedGameplaySystem
{
    public class Raycaster
    {
        private readonly RaycastSettings settings;

        public Raycaster(RaycastSettings settings)
        {
            this.settings = settings;
        }

        public Transform Raycast(Vector3 pos, Vector3 dir) 
        {
            return Raycast(pos, dir, settings.mask);
        }

        public Transform Raycast(Vector3 pos, Vector3 dir, LayerMask mask)
        {
            if (!Physics.Raycast(pos, dir, out RaycastHit hit, settings.range, mask, settings.triggerInteraction))
                return null;

            if (!settings.tags.Contains(hit.transform.tag))
                return null;

            return hit.transform;
        }
    }
}