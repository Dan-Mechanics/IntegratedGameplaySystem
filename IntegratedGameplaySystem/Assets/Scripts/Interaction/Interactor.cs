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
    public class Interactor : IStartable, IFixedUpdatable, IDisposable, IChangeTracker<string>
    {
        public event Action<string> OnChange;
        
        private readonly Raycaster raycaster;
        private readonly Transform cam;
        private readonly IInputService inputService;
        private readonly IWorldService worldService;
        private readonly Timer timer = new Timer();
        private readonly InputSource interacting;

        private string currentHover;

        /// <summary>
        /// Or we could push all of this upwards but that would make the game script messy.
        /// </summary>
        public Interactor()
        {
            cam = Camera.main.transform;
            raycaster = new Raycaster(ServiceLocator<IAssetService>.Locate().GetAssetByType<RaycastSettings>());
            inputService = ServiceLocator<IInputService>.Locate();
            worldService = ServiceLocator<IWorldService>.Locate();
            interacting = inputService.GetInputSource(PlayerAction.Interact);

            //timer.SetValue(0.1f);
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
            {
                currentHover = newHover;
                OnChange?.Invoke(currentHover);
            }

            if (interacting.IsPressed && timer.Tick(Time.fixedDeltaTime))
                Interact();

            /*if (inputService.GetInputSource(PlayerAction.Interact).IsPressed)
                TryInteract();*/
        }

        private void Interact()
        {
            timer.SetValue(0.1f);
            
            if (!raycaster.Raycast(cam.position, cam.forward, out Transform hitTransform))
                return;

            worldService.GetComponent<IInteractable>(hitTransform)?.Interact();
        }

        private IHoverable GetHoverable()
        {
            if (!raycaster.Raycast(cam.position, cam.forward, out Transform hitTransform))
                return null;

            //Debug.Log(worldService.Contains(hitTransform));

            return worldService.GetComponent<IHoverable>(hitTransform);
        }

        public void Dispose()
        {
            inputService.GetInputSource(PlayerAction.Interact).onDown -= Interact;
        }
    }
}