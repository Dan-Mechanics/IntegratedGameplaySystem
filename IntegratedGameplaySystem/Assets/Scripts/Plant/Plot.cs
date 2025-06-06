using System.Collections.Generic;
using UnityEngine;
using System;

namespace IntegratedGameplaySystem
{
    public class Plot : IPlantSpawner, IInteractable, IDisposable, IHoverable
    {
        public Func<int, bool> CanAffordUpgrade;
        public string HoverTitle => GetHoverTitle();

        //private readonly string title;
        private readonly PlotSettings settings;
        private readonly PlantFlyweight flyweight;
        private readonly Vector3 position;

        private GameObject button;
        private Plant[] plants;
        private bool isSprinkled;

        public Plot(PlotSettings settings, PlantFlyweight flyweight, int index)
        {
            this.settings = settings;
            this.flyweight = flyweight;

            //title = $"Sprinkler upgrade for {settings.upgradeCost}";
            position = new Vector3(index * settings.width + settings.spacing / 2f, 0f, settings.spacing / 2f);
        }

        public void Interact()
        {
            if (isSprinkled)
                return;

            if (!CanAffordUpgrade(settings.upgradeCost))
                return;

            ServiceLocator<IWorldService>.Locate().Remove(button);
            isSprinkled = true;
            EventManager<int>.RaiseEvent(Occasion.LoseMoney, settings.upgradeCost);
        }

        public void SpawnPlants(List<object> result)
        {
            button = Utils.SpawnPrefab(settings.buttonPrefab);
            button.transform.position += position;

            ServiceLocator<IWorldService>.Locate().Add(button, this);

            plants = new Plant[settings.width * settings.width];
            int i;

            for (int x = 0; x < settings.width; x++)
            {
                for (int z = 0; z < settings.width; z++)
                {
                    i = x + (z * settings.width);

                    plants[i] = new Plant(flyweight);
                    plants[i].HasSprinkler += HasSprinkler;

                    plants[i].transform.position += new Vector3(x * settings.spacing, 0f, z * settings.spacing) + position;
                    Utils.ApplyRandomRotation(plants[i].transform);

                    result.Add(plants[i]);
                }
            }

            result.Add(this);
        }

        private string GetHoverTitle() 
        {
            if (isSprinkled)
                return string.Empty;

            if (!CanAffordUpgrade(settings.upgradeCost))
                return "Can't afford yet!";

            return $"Sprinkler upgrade for {settings.upgradeCost}";
        }

        public void Dispose() 
        {
            for (int i = 0; i < plants.Length; i++)
            {
                plants[i].HasSprinkler -= HasSprinkler;
            }

            plants = null;
        }

        private bool HasSprinkler() 
        {
            return isSprinkled;
        }
    }
}