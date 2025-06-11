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

        public PlantUnit[] SpawnPlants(List<object> components)
        {
            PlantUnit[] plants = new PlantUnit[distribution.GetPlantCount()];

            for (int i = 0; i < plants.Length; i++)
            {
                plants[i] = new PlantUnit(flyweight);
                components.Add(plants[i]);
            }

            return plants;
        }
    }
}