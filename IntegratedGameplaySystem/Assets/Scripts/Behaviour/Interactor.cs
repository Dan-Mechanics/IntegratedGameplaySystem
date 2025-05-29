using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace IntegratedGameplaySystem
{
    /// <summary>
    /// We need:
    /// input
    /// world
    /// </summary>
    public class Interactor : IStartable, IFixedUpdatable, IDisposable
    {
        private readonly Raycaster raycaster;
        private readonly Transform cam;
        private readonly IInputService inputService;
        private readonly IWorldService worldService;

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
            inputService.GetInputSource(PlayerAction.PrimaryFire).OnDown += Interact;
        }

        public void FixedUpdate() => Hover();

        private void Interact()
        {
            Transform hit = raycaster.Raycast(cam.position, cam.forward);

            if (!hit)
                return;

            worldService.GetComponent<IInteractable>(hit.root.gameObject).Interact();
        }

        private void Hover()
        {
            Transform hit = raycaster.Raycast(cam.position, cam.forward);

            if (!hit)
                return;

            Debug.Log(hit.name + Time.time);
        }

        public void Dispose()
        {
            inputService.GetInputSource(PlayerAction.PrimaryFire).OnDown -= Interact;
        }
    }
}