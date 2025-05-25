using UnityEngine;

namespace IntegratedGameplaySystem
{
    public class CurrentSelection 
    {
        private readonly Raycaster raycaster;
        private readonly Transform eyes;

        public CurrentSelection(Raycaster raycaster, Transform eyes)
        {
            this.raycaster = raycaster;
            this.eyes = eyes;
        }

        public string GetSelection() 
        {
            Transform hit = raycaster.Raycast(eyes.position, eyes.forward);
            return hit ? hit.name : string.Empty;
        }
    }
}