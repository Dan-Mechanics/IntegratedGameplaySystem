using System.Collections.Generic;
using UnityEngine;

namespace IntegratedGameplaySystem
{
    public class Dispersal : IPlantSpawner
    {
        public readonly int plantCount;
        public readonly float dispersal;

        public Dispersal(int plantCount, float dispersal)
        {
            this.plantCount = plantCount;
            this.dispersal = dispersal;
        }

        public void Spawn(List<object> components, PlantBlueprint blueprint, Vector3 offset)
        {
            Plant temp;
            
            for (int i = 0; i < plantCount; i++)
            {
                temp = new Plant(blueprint);

                temp.transform.position += Utils.GetRandomFlatPos(dispersal);
                temp.transform.position += offset;
                Utils.ApplyRandomRotation(temp.transform);

                components.Add(temp);
            }
        }
    }
}