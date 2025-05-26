using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace IntegratedGameplaySystem
{
    public class Interactor
    {
        public const char SPLITTER = '_';
        
        private readonly InputHandler inputHandler;
        private readonly Raycaster raycaster;
        private readonly Transform eyes;
        private readonly PlayerContext playerContext;

        public Interactor(InputHandler inputHandler, Raycaster raycaster, Transform eyes, PlayerContext playerContext)
        {
            this.inputHandler = inputHandler;
            this.raycaster = raycaster;
            this.eyes = eyes;
            this.playerContext = playerContext;

            inputHandler.GetInputEvents(PlayerAction.PrimaryFire).OnDown += Interact;
        }

        private void Interact() 
        {
            Transform hit = raycaster.Raycast(eyes.position, eyes.forward);

            if (!hit)
                return;

            string[] split = hit.name.Split(SPLITTER);
            Plant plant = playerContext.plantConversions[split[0]].GetPlant(int.Parse(split[1]));
            plant.Interact();
            
        }

        public void Dispose()
        {
            inputHandler.GetInputEvents(PlayerAction.PrimaryFire).OnDown -= Interact;
        }
    }
}