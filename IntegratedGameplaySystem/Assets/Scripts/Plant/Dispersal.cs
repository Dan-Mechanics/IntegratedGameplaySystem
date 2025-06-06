using System.Collections.Generic;
using UnityEngine;

namespace IntegratedGameplaySystem
{
    /// <summary>
    /// This class is not my main class but you get the idea.
    /// </summary>
    public class Dispersal : IPlantDistribution, IDisposable
    {
        private readonly int plantCount;
        private readonly float dispersal;
        private readonly PlantFlyweight flyweight;
        private readonly Vector3 offset;

        private readonly List<Plant> plants = new();

        public Dispersal(int plantCount, float dispersal, PlantFlyweight flyweight, Vector3 offset)
        {
            this.plantCount = plantCount;
            this.dispersal = dispersal;
            this.flyweight = flyweight;
            this.offset = offset;
        }

        public void Dispose()
        {
            plants.ForEach(x => x.IsAlwaysWatered -= Dummy);
        }

        public void SpawnPlants(List<object> components)
        {
            components.Add(this);
            
            Plant plant;
            
            for (int i = 0; i < plantCount; i++)
            {
                plant = new Plant(flyweight);

                plant.transform.position += Utils.GetRandomFlatPos(dispersal);
                plant.transform.position += offset;
                Utils.ApplyRandomRotation(plant.transform);
                plant.IsAlwaysWatered += Dummy;

                plants.Add(plant);
                components.Add(plant);
            }
        }

        private bool Dummy()
        {
            return false;
        }
    }
}