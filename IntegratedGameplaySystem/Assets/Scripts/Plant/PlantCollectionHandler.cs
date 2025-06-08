using System.Collections.Generic;
using UnityEngine;

namespace IntegratedGameplaySystem
{
    /// <summary>
    /// Extract to plantupgrades so that we can have both dipseral and other hsit.
    /// What is this class even for ??
    /// </summary>
    public class PlantCollectionHandler 
    {
        private readonly PlantFlyweight flyweight;
        private readonly IPlantPlacementStrategy strategy;
        private readonly int index;

        private Plant[] plants;

        /// <summary>
        /// Builder ??
        /// </summary>
        public PlantCollectionHandler(int index, PlantFlyweight flyweight, IPlantPlacementStrategy strategy)
        {
            this.index = index;
            this.flyweight = flyweight;
            this.strategy = strategy;
        }

        public void SpawnPlants(List<object> components)
        {
            //components.Add(this);

            plants = new Plant[strategy.GetPlantCount()];

            for (int i = 0; i < plants.Length; i++)
            {
                plants[i] = new Plant(flyweight);
                components.Add(plants[i]);
            }

            strategy.PlacePlants(plants);
        }
    }
}