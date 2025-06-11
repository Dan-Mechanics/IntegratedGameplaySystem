using System;
using UnityEngine;

namespace IntegratedGameplaySystem
{
    /// <summary>
    /// The player can interact with the world.
    /// </summary>
    public class Interactor : IStartable, IFixedUpdatable, IDisposable, IChangeTracker<string>
    {
        public event Action<string> OnChange;
        
        private readonly Raycaster raycaster;
        private readonly Transform cam;
        private readonly IInputService inputService;
        private readonly IWorldService worldService;

        private string currentHover;

        public Interactor()
        {
            cam = Camera.main.transform;
            raycaster = new Raycaster(ServiceLocator<IAssetService>.Locate().GetAssetByType<RaycastSettings>());
            inputService = ServiceLocator<IInputService>.Locate();
            worldService = ServiceLocator<IWorldService>.Locate();
        }

        public void Start() 
        {
            inputService.GetInputSource(PlayerAction.Interact).onDown += Interact;
            OnChange?.Invoke(string.Empty);
        }

        public void FixedUpdate() 
        {
            string newHover = GetHoverable()?.GetHoverTitle();

            if (newHover == currentHover)
                return;

            currentHover = newHover;
            OnChange?.Invoke(currentHover);
        }

        private void Interact()
        {
            if (!raycaster.Raycast(cam.position, cam.forward, out Transform hitTransform))
                return;

            worldService.GetComponent<IInteractable>(hitTransform)?.Interact();
        }

        private IHoverable GetHoverable()
        {
            if (!raycaster.Raycast(cam.position, cam.forward, out Transform hitTransform))
                return null;

            return worldService.GetComponent<IHoverable>(hitTransform);
        }

        public void Dispose()
        {
            inputService.GetInputSource(PlayerAction.Interact).onDown -= Interact;
        }
    }
}