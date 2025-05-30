using System;
using UnityEngine;

namespace IntegratedGameplaySystem
{
    /// <summary>
    /// We need:
    /// input
    /// world
    /// </summary>
    public class Interactor : IStartable, IFixedUpdatable, IDisposable
    {
        public event Action<string> OnHoverChange;
        
        private readonly Raycaster raycaster;
        private readonly Transform cam;
        private readonly IInputService inputService;
        private readonly IWorldService worldService;

        private string prevHovering;
        private string hovering;

        /// <summary>
        /// Or we could push the asset name upward.
        /// </summary>
        public Interactor()
        {
            // or something idk.
            raycaster = new Raycaster(ServiceLocator<IAssetService>.Locate().GetByType<RaycastSettings>());
            cam = Camera.main.transform;

            inputService = ServiceLocator<IInputService>.Locate();
            worldService = ServiceLocator<IWorldService>.Locate();
        }

        public void Start() 
        {
            inputService.GetInputSource(PlayerAction.Interact).onDown += InteractWithWorld;
            SetHovering(string.Empty);
        }

        public void FixedUpdate() => Hover();

        private void InteractWithWorld()
        {
            if (!DoRaycast(out Transform hit))
                return;

            worldService.GetComponent<IInteractable>(hit.gameObject).Interact();
        }

        private bool DoRaycast(out Transform hit)
        {
            hit = raycaster.Raycast(cam.position, cam.forward);
            return hit;
        }

        private void Hover()
        {
            if (!DoRaycast(out Transform hit))
                return;

            hovering = hit.name;
            if (hovering == prevHovering)
                return;

            SetHovering(hovering);
        }

        private void SetHovering(string hovering)
        {
            OnHoverChange?.Invoke(hovering);
            prevHovering = hovering;
        }

        public void Dispose()
        {
            inputService.GetInputSource(PlayerAction.Interact).onDown -= InteractWithWorld;
        }
    }
}