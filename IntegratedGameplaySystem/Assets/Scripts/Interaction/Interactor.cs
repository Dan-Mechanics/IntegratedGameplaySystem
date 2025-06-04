using System;
using UnityEngine;

namespace IntegratedGameplaySystem
{
    /// <summary>
    /// Input:
    /// input
    /// world
    /// 
    /// Output:
    /// OnHoverChange
    /// IInteractable.interact.
    /// </summary>
    public class Interactor : IStartable, IFixedUpdatable, IDisposable
    {
        public event Action<string> OnHoverChange;
        
        private readonly Raycaster raycaster;
        private readonly Transform cam;
        private readonly IInputService inputService;
        private readonly IWorldService worldService;

        private string currentHover;

        /// <summary>
        /// Or we could push all of this upwards but that would make the game script messy.
        /// </summary>
        public Interactor()
        {
            cam = Camera.main.transform;
            raycaster = new Raycaster(ServiceLocator<IAssetService>.Locate().GetAssetWithType<RaycastSettings>());
            inputService = ServiceLocator<IInputService>.Locate();
            worldService = ServiceLocator<IWorldService>.Locate();
        }

        public void Start() 
        {
            inputService.GetInputSource(PlayerAction.Interact).onDown += TryInteract;
            OnHoverChange?.Invoke(string.Empty);
        }

        public void FixedUpdate() 
        {
            string newHover = GetHoverable(out IHoverable hoverable) ? hoverable.HoverTitle : string.Empty;

            if (newHover == currentHover)
                return;

            currentHover = newHover;
            OnHoverChange?.Invoke(currentHover);
        }

        private void TryInteract()
        {
            if (!raycaster.Raycast(cam.position, cam.forward, out Transform hitTransform))
                return;

            worldService.GetComponent<IInteractable>(hitTransform)?.Interact();
        }

        private bool GetHoverable(out IHoverable hoverable)
        {
            hoverable = null;

            if (!raycaster.Raycast(cam.position, cam.forward, out Transform hitTransform))
                return false;

            hoverable = worldService.GetComponent<IHoverable>(hitTransform);
            return true;
        }

        public void Dispose()
        {
            inputService.GetInputSource(PlayerAction.Interact).onDown -= TryInteract;
        }
    }
}