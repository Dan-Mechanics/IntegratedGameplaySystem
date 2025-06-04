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

        private string currentlyHovering;
        private LayerMask defaultMask;
        private LayerMask mask;

        /// <summary>
        /// Or we could push the asset name upward.
        /// </summary>
        public Interactor()
        {
            // or something idk.

            RaycastSettings settings = ServiceLocator<IAssetService>.Locate().GetAssetWithType<RaycastSettings>();
            raycaster = new Raycaster(settings);
            defaultMask = settings.mask;
            mask = defaultMask;

            cam = Camera.main.transform;

            inputService = ServiceLocator<IInputService>.Locate();
            worldService = ServiceLocator<IWorldService>.Locate();
        }

        public void Start() 
        {
            inputService.GetInputSource(PlayerAction.Interact).onDown += TryInteract;
            EventManager<IItem>.AddListener(Occasion.EquipItem, OnNewItem);

            OnHoverChange?.Invoke(string.Empty);
        }

        public void OnNewItem(IItem item) 
        {
            if (item == null)
            {
                mask = defaultMask;
                return;
            }

            mask = item.Mask;
        }

        public void FixedUpdate() 
        {
            IHoverable hit = GetHoverHit();
            string newHover = hit == null ? string.Empty : hit.HoverTitle;

            if (newHover == currentlyHovering)
                return;

            currentlyHovering = newHover;
            OnHoverChange?.Invoke(currentlyHovering);
        }

        private void TryInteract()
        {
            Transform hit = raycaster.Raycast(cam.position, cam.forward, mask);

            if (!hit)
                return;

            worldService.GetComponent<IInteractable>(hit)?.Interact();
        }

        private IHoverable GetHoverHit()
        {
            Transform hit = raycaster.Raycast(cam.position, cam.forward, mask);

            if (!hit)
                return null;

            return worldService.GetComponent<IHoverable>(hit);
        }

        public void Dispose()
        {
            inputService.GetInputSource(PlayerAction.Interact).onDown -= TryInteract;
            EventManager<IItem>.RemoveListener(Occasion.EquipItem, OnNewItem);
        }
    }
}