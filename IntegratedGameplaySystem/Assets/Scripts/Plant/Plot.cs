using System.Collections.Generic;
using UnityEngine;

namespace IntegratedGameplaySystem
{
    /// <summary>
    /// Extract to plantupgrades so that we can have both dipseral and other hsit.
    /// </summary>
    public class Plot : IPlantDistribution, IDisposable, IStartable
    {
        private readonly PermanentUpgrade sprinkler = new();
        private readonly RepeatableUpgrade grenade = new();
        
        private readonly PlotSettings settings;
        private readonly Vector3 position;
        private readonly MoneyCentral money;

        private Plant[] plantsOnPlot;

        public Plot(PlotSettings settings, int index, MoneyCentral money)
        {
            this.settings = settings;
            this.money = money;

            position = new Vector3(index * settings.width + settings.spacing / 2f, 0f, settings.spacing / 2f);
        }

        public void SpawnPlants(List<object> components)
        {
            components.Add(this);
            
            plantsOnPlot = new Plant[settings.width * settings.width];
            int index;

            for (int x = 0; x < settings.width; x++)
            {
                for (int z = 0; z < settings.width; z++)
                {
                    index = x + (z * settings.width);
                    plantsOnPlot[index] = new Plant(settings.plant);

                    plantsOnPlot[index].transform.position += new Vector3(x * settings.spacing, 0f, z * settings.spacing) + position;
                    Utils.ApplyRandomRotation(plantsOnPlot[index].transform);
                    components.Add(plantsOnPlot[index]);
                }
            }
        }

        public void Start() 
        {
            IWorldService world = ServiceLocator<IWorldService>.Locate();

            sprinkler.SetValues(settings.sprinkler);
            grenade.SetValues(settings.grenade);

            sprinkler.Setup(world, position);
            grenade.Setup(world, position);

            sprinkler.OnBuy += RefreshAllRainEffects;
            sprinkler.OnCanAfford += money.CanAfford;
            grenade.OnCanAfford += money.CanAfford;
            grenade.OnBuy += HarvestAll;

            for (int i = 0; i < plantsOnPlot.Length; i++)
            {
                plantsOnPlot[i].IsAlwaysWatered += sprinkler.GetHasBought;
            }
        }

        private void HarvestAll() 
        {
            for (int i = 0; i < plantsOnPlot.Length; i++)
            {
                plantsOnPlot[i].TryHarvest();
            }
        }

        private void RefreshAllRainEffects() 
        {
            for (int i = 0; i < plantsOnPlot.Length; i++)
            {
                plantsOnPlot[i].RefreshRainEffects();
            }
        }

        public void Dispose() 
        {
            sprinkler.OnBuy -= RefreshAllRainEffects;
            sprinkler.OnCanAfford -= money.CanAfford;
            grenade.OnCanAfford -= money.CanAfford;
            grenade.OnBuy -= HarvestAll;

            for (int i = 0; i < plantsOnPlot.Length; i++)
            {
                plantsOnPlot[i].IsAlwaysWatered -= sprinkler.GetHasBought;
            }
        }
    }
}