using System.Collections.Generic;
using UnityEngine;

namespace IntegratedGameplaySystem
{
    public class Dispersal : IPlantSpawner
    {
        public readonly int plantCount;
        public readonly float dispersal;
        public readonly PlantFlyweight blueprint;
        public readonly Vector3 offset;

        public Dispersal(int plantCount, float dispersal, PlantFlyweight blueprint, Vector3 offset)
        {
            this.plantCount = plantCount;
            this.dispersal = dispersal;
            this.blueprint = blueprint;
            this.offset = offset;
        }

        public void Spawn(List<object> result)
        {
            Plant plant;
            
            for (int i = 0; i < plantCount; i++)
            {
                plant = new Plant(blueprint);

                plant.transform.position += Utils.GetRandomFlatPos(dispersal);
                plant.transform.position += offset;
                Utils.ApplyRandomRotation(plant.transform);

                result.Add(plant);
            }
        }
    }
}