using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace IntegratedGameplaySystem
{
    [CreateAssetMenu(menuName = nameof(BaseBehaviour) + "/" + nameof(InteractBehaviour), fileName = "New " + nameof(InteractBehaviour))]
    public class InteractBehaviour : BaseBehaviour
    {
        public const char SPLITTER = '_';

        public Raycaster.RaycastData data;
        public PatchBehaviour[] patchBehaviours;
        private readonly Dictionary<string, PatchBehaviour> plantConversions = new Dictionary<string, PatchBehaviour>();

        private InputHandler inputHandler; // need from somewhere.
        private Raycaster raycaster;
        private Transform cam;

        public override void Start()
        {
            base.Start();

            for (int i = 0; i < patchBehaviours.Length; i++)
            {
                plantConversions.Add(patchBehaviours[i].name, patchBehaviours[i]);
            }

            raycaster = new Raycaster(data);
            cam = Camera.main.transform;
            //inputHandler.GetInputEvents(PlayerAction.PrimaryFire).OnDown += Interact;
        }

        private void Interact() 
        {
            Transform hit = raycaster.Raycast(cam.position, cam.forward);

            if (!hit)
                return;

            string[] split = hit.name.Split(SPLITTER);
            Plant plant = plantConversions[split[0]].GetPlant(int.Parse(split[1]));
            plant.Interact();
        }

        public void SetInputHandler(InputHandler inputHandler) 
        {
            this.inputHandler = inputHandler;
            inputHandler.GetInputEvents(PlayerAction.PrimaryFire).OnDown += Interact;
        }

        public override void Disable()
        {
            base.Disable();
            inputHandler.GetInputEvents(PlayerAction.PrimaryFire).OnDown -= Interact;
        }
    }
}