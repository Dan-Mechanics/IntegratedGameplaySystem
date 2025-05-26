using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace IntegratedGameplaySystem
{
    [CreateAssetMenu(menuName = nameof(BaseBehaviour) + "/" + nameof(InteractBehaviour), fileName = "New " + nameof(InteractBehaviour))]
    public class InteractBehaviour : BaseBehaviour
    {
        public const char SPLITTER = '_';

        //public InputBehaviour inputBehaviour;
        public DisplayBehaviour displayBehaviour;
        public PatchBehaviour[] patchBehaviours;
        public Raycaster.RaycastData data;

        private readonly Dictionary<string, PatchBehaviour> plantConversions = new();
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

            ServiceLocator<IInputService>.Locate().GetAction(PlayerAction.PrimaryFire).OnDown += Interact;
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            Hover();
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

        private void Hover()
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
        }
    }
}