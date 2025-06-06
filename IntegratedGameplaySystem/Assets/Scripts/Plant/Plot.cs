using System.Collections.Generic;
using UnityEngine;
using System;

namespace IntegratedGameplaySystem
{
    /// <summary>
    /// Extract to plantupgrades so that we can have both dipseral and other hsit.
    /// </summary>
    public class Plot : IPlantSpawner, IDisposable
    {
        private readonly PlotSettings settings;
        private readonly PlantFlyweight flyweight;
        private readonly Vector3 position;
        private readonly MoneyCentral money;

        private Plant[] plantOnPlot;
        private PermaUpgrade sprinkler;

        public Plot(PlotSettings settings, PlantFlyweight flyweight, int index, MoneyCentral money)
        {
            this.settings = settings;
            this.flyweight = flyweight;
            this.money = money;

            position = new Vector3(index * settings.width + settings.spacing / 2f, 0f, settings.spacing / 2f);
        }

        public void Spawn(List<object> result)
        {
            GameObject sprinklerButton = Utils.SpawnPrefab(settings.sprinklerButtonPrefab);
            sprinklerButton.transform.position += position;

            sprinkler = new PermaUpgrade("Sprinkler", settings.sprinklerCost, sprinklerButton, ServiceLocator<IWorldService>.Locate());
            //sprinkler.OnHovering += GetSprinklerHoverTitle;
            sprinkler.OnCanAfford += money.CanAfford;

            plantOnPlot = new Plant[settings.width * settings.width];
            int index;

            for (int x = 0; x < settings.width; x++)
            {
                for (int z = 0; z < settings.width; z++)
                {
                    index = x + (z * settings.width);

                    plantOnPlot[index] = new Plant(flyweight);
                    plantOnPlot[index].IsAlwaysWatered += sprinkler.GetHasBought;

                    plantOnPlot[index].transform.position += new Vector3(x * settings.spacing, 0f, z * settings.spacing) + position;
                    Utils.ApplyRandomRotation(plantOnPlot[index].transform);

                    result.Add(plantOnPlot[index]);
                }
            }

            result.Add(this);
        }

        public void Dispose() 
        {
            for (int i = 0; i < plantOnPlot.Length; i++)
            {
                plantOnPlot[i].IsAlwaysWatered -= sprinkler.GetHasBought;
            }

            sprinkler.OnCanAfford -= money.CanAfford;
        }
    }
}