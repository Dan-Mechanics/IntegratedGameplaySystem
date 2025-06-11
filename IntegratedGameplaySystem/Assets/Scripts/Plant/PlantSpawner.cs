using System.Collections.Generic;

namespace IntegratedGameplaySystem
{
    public class PlantSpawner 
    {
        private readonly PlantDistribution distribution;
        private PlantFlyweight flyweight;

        public PlantSpawner(PlantDistribution distribution)
        {
            this.distribution = distribution;
        }

        public void SetPlant(PlantFlyweight flyweight) => this.flyweight = flyweight;

        public PlantCommonality[] SpawnPlants(List<object> components)
        {
            PlantCommonality[] plants = new PlantCommonality[distribution.GetPlantCount()];

            for (int i = 0; i < plants.Length; i++)
            {
                plants[i] = new PlantCommonality(flyweight);
                components.Add(plants[i]);
            }

            return plants;
        }
    }
}