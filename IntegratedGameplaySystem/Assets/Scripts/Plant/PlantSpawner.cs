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
        private readonly PlantDistribution distribution;
        private PlantFlyweight flyweight;

        public PlantSpawner(PlantDistribution distribution)
        {
            this.distribution = distribution;
        }

        public void SetPlant(PlantFlyweight flyweight) => this.flyweight = flyweight;

        public SoilUnit[] SpawnPlants(List<object> components)
        {
            SoilUnit[] plants = new SoilUnit[distribution.GetPlantCount()];

            for (int i = 0; i < plants.Length; i++)
            {
                plants[i] = new SoilUnit(flyweight);
                components.Add(plants[i]);
            }

            //distribution.PlacePlants(plants);

            return plants;
        }
    }
}