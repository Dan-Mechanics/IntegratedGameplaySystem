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

        public bool Raycast(Vector3 pos, Vector3 dir, out Transform hitTransform) 
        {
            hitTransform = null;
            
            if (!Physics.Raycast(pos, dir, out RaycastHit hit, settings.range, settings.mask, settings.interaction))
                return false;

            if (!settings.tags.Contains(hit.transform.tag))
                return false;

            hitTransform = hit.transform;
            return true;
        }
    }
}