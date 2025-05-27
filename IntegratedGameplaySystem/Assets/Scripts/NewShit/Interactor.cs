using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace IntegratedGameplaySystem
{
    /// <summary>
    /// We need:
    /// input
    /// world
    /// </summary>
    public class Interactor : IStartable, IDisposable
    {
        private Raycaster raycaster;
        private Transform cam;
        private IInputService inputService;
        private IWorldService worldService;

        public Interactor()
        {
            // or something idk.
            raycaster = new Raycaster(Resources.Load<RaycastData>("Data/interactor_raycast"));
            cam = Camera.main.transform;

            inputService = ServiceLocator<IInputService>.Locate();
            worldService = ServiceLocator<IWorldService>.Locate();
        }

        public void Start() 
        {
            inputService.GetInputSource(PlayerAction.PrimaryFire).OnDown += Interact;
        }

        /*public override void FixedUpdate()
        {
            base.FixedUpdate();

            Hover();
        }*/

        private void Interact()
        {
            Transform hit = raycaster.Raycast(cam.position, cam.forward);

            if (!hit)
                return;

            worldService.GetComponent<IInteractable>(hit.gameObject).Interact();
        }

        /*private void Hover()
        {
            Transform hit = raycaster.Raycast(cam.position, cam.forward);

            if (!hit)
                return;

            displayBehaviour.WriteText(hit.name);
        }

        public override void Disable()
        {
            base.Disable();
            ServiceLocator<IInputService>.Locate().GetAction(PlayerAction.PrimaryFire).OnDown -= Interact;
        }*/

        public void Dispose()
        {
            inputService.GetInputSource(PlayerAction.PrimaryFire).OnDown -= Interact;
        }
    }
}