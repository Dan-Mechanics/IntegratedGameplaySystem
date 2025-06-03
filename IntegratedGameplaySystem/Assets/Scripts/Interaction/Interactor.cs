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
        public event Action<Transform> OnHoverChange;
        
        private readonly Raycaster raycaster;
        private readonly Transform cam;
        private readonly IInputService inputService;
        private readonly IWorldService worldService;

        private Transform currentlyHovering;

        /// <summary>
        /// Or we could push the asset name upward.
        /// </summary>
        public Interactor()
        {
            // or something idk.
            raycaster = new Raycaster(ServiceLocator<IAssetService>.Locate().GetAssetByType<RaycastSettings>());
            cam = Camera.main.transform;

            inputService = ServiceLocator<IInputService>.Locate();
            worldService = ServiceLocator<IWorldService>.Locate();
        }

        public void Start() 
        {
            inputService.GetInputSource(PlayerAction.Interact).onDown += InteractWithWorld;
        }

        public void FixedUpdate() => Hover();

        private void InteractWithWorld()
        {
            if (!DoRaycast(out Transform hit))
                return;

            worldService.GetComponent<IInteractable>(hit.gameObject)?.Interact();
        }

        private bool DoRaycast(out Transform hit)
        {
            hit = raycaster.Raycast(cam.position, cam.forward);
            return hit;
        }

        private void Hover()
        {
            DoRaycast(out Transform hit);

            if (hit == currentlyHovering)
                return;

            currentlyHovering = hit;
            OnHoverChange?.Invoke(currentlyHovering);
        }

        public void Dispose()
        {
            inputService.GetInputSource(PlayerAction.Interact).onDown -= InteractWithWorld;
        }
    }
}