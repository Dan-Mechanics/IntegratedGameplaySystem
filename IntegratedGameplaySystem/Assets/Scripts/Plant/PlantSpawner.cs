using System.Collections.Generic;
using UnityEngine;

namespace IntegratedGameplaySystem
{
    /// <summary>
    /// Extract to plantupgrades so that we can have both dipseral and other hsit.
    /// What is this class even for ??
    /// </summary>
    public class PlantSpawner 
    {
        private readonly IPlantDistributionStrategy strategy;
        private PlantFlyweight flyweight;

        public PlantSpawner( IPlantDistributionStrategy strategy)
        {
            //this.flyweight = flyweight;
            this.strategy = strategy;
        }

        public void SetPlant(PlantFlyweight flyweight) => this.flyweight = flyweight;

        public void SpawnPlants(List<object> components)
        {
            SoilUnit[] plants = new SoilUnit[strategy.GetPlantCount()];

            for (int i = 0; i < plants.Length; i++)
            {
                plants[i] = new SoilUnit(flyweight);
                components.Add(plants[i]);
            }

            strategy.PlacePlants(plants);
        }
    }
}