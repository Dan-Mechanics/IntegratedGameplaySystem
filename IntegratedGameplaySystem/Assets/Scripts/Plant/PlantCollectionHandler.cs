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
        private readonly List<IUpgradable> upgradables = new();

        private readonly PlantFlyweight flyweight;
        private readonly MoneyCentral money;
        private readonly IPlantPlacementStrategy strategy;
        private readonly int index;
        private readonly UpgradeSettings settings;

        private Plant[] plants;

        /// <summary>
        /// Builder ??
        /// </summary>
        public PlantCollectionHandler(UpgradeSettings settings, int index, PlantFlyweight flyweight, MoneyCentral money, IPlantPlacementStrategy strategy)
        {
            this.index = index;
            this.settings = settings;
            this.flyweight = flyweight;
            this.money = money;
            this.strategy = strategy;

            GiveValues(sprinkler, settings.sprinkler);
            GiveValues(grenade, settings.grenade);

            upgradables.Add(sprinkler);
            upgradables.Add(grenade);
        }

        private void GiveValues(IUpgradable upgradable, UpgradeValuesInspector values) => upgradable.SetValues(values);

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
            IWorldService world = ServiceLocator<IWorldService>.Locate();
            Vector3 offset = strategy.GetPlotPos(index);

            upgradables.ForEach(x => x.Setup(offset, world));

            sprinkler.OnBuy += InstallSprinkler;
            grenade.OnBuy += UseGrenade;
            upgradables.ForEach(x => x.OnCanBuy += money.CanAfford);
        }

        /// <summary>
        /// Harvest all ...
        /// </summary>
        private void UseGrenade() 
        {
            for (int i = 0; i < plants.Length; i++)
            {
                plants[i].Harvest();
            }

            Transform effect = Utils.SpawnPrefab(settings.grenadeEffect).transform;
            effect.position = strategy.GetPlotPos(index);
        }

        /// <summary>
        /// Make the whole plot rain ...
        /// </summary>
        private void InstallSprinkler() 
        {
            for (int i = 0; i < plants.Length; i++)
            {
                plants[i].Water();
            }
        }

        public void Dispose() 
        {
            sprinkler.OnBuy -= InstallSprinkler;
            grenade.OnBuy -= UseGrenade;
            upgradables.ForEach(x => x.OnCanBuy -= money.CanAfford);
        }
    }
}