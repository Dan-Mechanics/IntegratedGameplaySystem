using System.Collections.Generic;
using UnityEngine;

namespace IntegratedGameplaySystem
{
    /// <summary>
    /// Extract to plantupgrades so that we can have both dipseral and other hsit.
    /// </summary>
    public class PlantHolder : IDisposable, IStartable
    {
        private readonly PermanentUpgrade sprinkler = new();
        private readonly RepeatableUpgrade grenade = new();

        private readonly PlantFlyweight flyweight;
        private readonly MoneyCentral money;
        private readonly IPlantPlacementStrategy strategy;
        private readonly int index;

        private Plant[] plantsOnPlot;

        /// <summary>
        /// Builder ??
        /// </summary>
        public PlantHolder(UpgradeSettings upgradeSettings, int index, PlantFlyweight flyweight, MoneyCentral money, IPlantPlacementStrategy strategy)
        {
            this.index = index;
            this.flyweight = flyweight;
            this.money = money;
            this.strategy = strategy;


            IWorldService world = ServiceLocator<IWorldService>.Locate();
            Vector3 plotPos = strategy.GetPlotPos(index);

            sprinkler.SetValues(upgradeSettings.sprinkler);
            grenade.SetValues(upgradeSettings.grenade);
            
            sprinkler.Setup(world, plotPos);
            grenade.Setup(world, plotPos);
        }

        public void SpawnPlants(List<object> components)
        {
            components.Add(this);

            plantsOnPlot = new Plant[strategy.GetPlantCount()];

            for (int i = 0; i < plantsOnPlot.Length; i++)
            {
                plantsOnPlot[i] = new Plant(flyweight);
                components.Add(plantsOnPlot[i]);
            }

            strategy.PlacePlants(plantsOnPlot, index);
        }

        public void Start() 
        {
            sprinkler.OnBuy += InstallSprinkler;
            sprinkler.OnCanAfford += money.CanAfford;
            grenade.OnCanAfford += money.CanAfford;
            grenade.OnBuy += UseGrenade;
        }

        /// <summary>
        /// Harvest all.
        /// </summary>
        private void UseGrenade() 
        {
            for (int i = 0; i < plantsOnPlot.Length; i++)
            {
                plantsOnPlot[i].TryHarvest();
            }
        }

        /// <summary>
        /// Plot always wet.
        /// </summary>
        private void InstallSprinkler() 
        {
            for (int i = 0; i < plantsOnPlot.Length; i++)
            {
                plantsOnPlot[i].SetAlwaysWatered(true);
                //plantsOnPlot[i].RefreshRainEffects();
            }
        }

        public void Dispose() 
        {
            sprinkler.OnBuy -= InstallSprinkler;
            sprinkler.OnCanAfford -= money.CanAfford;
            grenade.OnCanAfford -= money.CanAfford;
            grenade.OnBuy -= UseGrenade;
        }
    }
}