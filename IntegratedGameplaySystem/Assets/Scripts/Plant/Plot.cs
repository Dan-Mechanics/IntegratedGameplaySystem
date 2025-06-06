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

        private Plant[] plantsOnPlot;
        private PermaUpgrade sprinkler = new PermaUpgrade();

        public Plot(PlotSettings settings, PlantFlyweight flyweight, int index, MoneyCentral money)
        {
            this.settings = settings;
            this.flyweight = flyweight;
            this.money = money;

            position = new Vector3(index * settings.width + settings.spacing / 2f, 0f, settings.spacing / 2f);
        }

        public void SetupPlot(List<object> result)
        {
            IWorldService world = ServiceLocator<IWorldService>.Locate();

            sprinkler.Setup(settings.sprinkler, world, position);
            //sprinkler.OnHovering += GetSprinklerHoverTitle;
            sprinkler.OnCanAfford += money.CanAfford;

            plantsOnPlot = new Plant[settings.width * settings.width];
            int index;

            for (int x = 0; x < settings.width; x++)
            {
                for (int z = 0; z < settings.width; z++)
                {
                    index = x + (z * settings.width);

                    plantsOnPlot[index] = new Plant(flyweight);
                    plantsOnPlot[index].IsAlwaysWatered += sprinkler.GetHasBought;

                    plantsOnPlot[index].transform.position += new Vector3(x * settings.spacing, 0f, z * settings.spacing) + position;
                    Utils.ApplyRandomRotation(plantsOnPlot[index].transform);

                    result.Add(plantsOnPlot[index]);
                }
            }

            result.Add(this);
        }

        public void Dispose() 
        {
            for (int i = 0; i < plantsOnPlot.Length; i++)
            {
                plantsOnPlot[i].IsAlwaysWatered -= sprinkler.GetHasBought;
            }

            sprinkler.OnCanAfford -= money.CanAfford;
        }
    }
}