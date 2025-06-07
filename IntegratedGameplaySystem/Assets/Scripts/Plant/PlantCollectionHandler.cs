using System.Collections.Generic;
using UnityEngine;

namespace IntegratedGameplaySystem
{
    /// <summary>
    /// Extract to plantupgrades so that we can have both dipseral and other hsit.
    /// </summary>
    public class PlantCollectionHandler : IDisposable, IStartable
    {
        private readonly PermanentUpgrade sprinkler = new();
        private readonly TemporaryUpgrade grenade = new();

        private readonly PlantFlyweight flyweight;
        private readonly MoneyCentral money;
        private readonly IPlantPlacementStrategy strategy;
        private readonly int index;

        private Plant[] plants;

        /// <summary>
        /// Builder ??
        /// </summary>
        public PlantCollectionHandler(UpgradeSettings upgradeSettings, int index, PlantFlyweight flyweight, MoneyCentral money, IPlantPlacementStrategy strategy)
        {
            this.index = index;
            this.flyweight = flyweight;
            this.money = money;
            this.strategy = strategy;


            IWorldService world = ServiceLocator<IWorldService>.Locate();
            Vector3 offset = strategy.GetPlotPos(index);

            sprinkler.Setup(world, offset, upgradeSettings.sprinkler);
            grenade.Setup(world, offset, upgradeSettings.grenade);
        }

        public void SpawnPlants(List<object> components)
        {
            components.Add(this);

            plants = new Plant[strategy.GetPlantCount()];

            for (int i = 0; i < plants.Length; i++)
            {
                plants[i] = new Plant(flyweight);
                components.Add(plants[i]);
            }

            strategy.PlacePlants(plants, index);
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
            for (int i = 0; i < plants.Length; i++)
            {
                plants[i].TryHarvest();
            }
        }

        /// <summary>
        /// Plot always wet.
        /// </summary>
        private void InstallSprinkler() 
        {
            for (int i = 0; i < plants.Length; i++)
            {
                plants[i].SetAlwaysWatered(true);
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