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
        public event Action<IHoverable> OnHoverChange;
        
        private readonly Raycaster raycaster;
        private readonly Transform cam;
        private readonly IInputService inputService;
        private readonly IWorldService worldService;

        private IHoverable currentlyHovering;

        /// <summary>
        /// Or we could push the asset name upward.
        /// </summary>
        public Interactor()
        {
            // or something idk.
            raycaster = new Raycaster(ServiceLocator<IAssetService>.Locate().GetAssetWithType<RaycastSettings>());
            cam = Camera.main.transform;

            inputService = ServiceLocator<IInputService>.Locate();
            worldService = ServiceLocator<IWorldService>.Locate();
        }

        public void Start() 
        {
            inputService.GetInputSource(PlayerAction.Interact).onDown += TryInteract;
            OnHoverChange?.Invoke(null);
        }

        public void FixedUpdate() 
        {
            IHoverable hoverable = GetHoverHit();

            if (hoverable == currentlyHovering)
                return;

            currentlyHovering = hoverable;
            OnHoverChange?.Invoke(currentlyHovering);
        }

        private void TryInteract()
        {
            Transform hit = raycaster.Raycast(cam.position, cam.forward);

            if (!hit)
                return;

            worldService.GetComponent<IInteractable>(hit)?.Interact();
        }

        private IHoverable GetHoverHit()
        {
            Transform hit = raycaster.Raycast(cam.position, cam.forward);

            if (!hit)
                return null;

            return worldService.GetComponent<IHoverable>(hit);
        }

        public void Dispose()
        {
            inputService.GetInputSource(PlayerAction.Interact).onDown -= TryInteract;
        }
    }
}