using System.Collections.Generic;
using UnityEngine;

namespace IntegratedGameplaySystem
{
    /// <summary>
    /// This shit is getting a little messy again GRRR.
    /// </summary>
    public interface IPlantSpawner 
    {
        public void Spawn(List<object> behaviours, PlantBlueprint blueprint, Vector3 offset, List<Plant> plants);
    }

    public class Dispersal : IPlantSpawner
    {
        public readonly int plantCount;
        public readonly float dispersal;

        public Dispersal(int plantCount, float dispersal)
        {
            this.plantCount = plantCount;
            this.dispersal = dispersal;
        }

        public void Spawn(List<object> behaviours, PlantBlueprint blueprint, Vector3 offset, List<Plant> plants)
        {
            Plant temp;
            
            for (int i = 0; i < plantCount; i++)
            {
                temp = new Plant(blueprint);

                temp.sceneObject.transform.position += Utils.GetRandomFlatPos(dispersal);
                temp.sceneObject.transform.position += offset;
                Utils.ApplyRandomRotation(temp.sceneObject.transform);

                plants.Add(temp);
                behaviours.Add(temp);
            }
        }
    }

    public class Plot : IPlantSpawner
    {
        public readonly int width;
        public readonly float spacing;

        public Plot(int width, float spacing)
        {
            this.width = width;
            this.spacing = spacing;
        }

        public void Spawn(List<object> behaviours, PlantBlueprint blueprint, Vector3 offset, List<Plant> plants)
        {
            Plant temp;

            for (int x = 0; x < width; x++)
            {
                for (int z = 0; z < width; z++)
                {
                    temp = new Plant(blueprint);

                    temp.sceneObject.transform.position += new Vector3(x * spacing, 0f, z * spacing);
                    temp.sceneObject.transform.position += offset;
                    Utils.ApplyRandomRotation(temp.sceneObject.transform);

                    plants.Add(temp);
                    behaviours.Add(temp);
                }
            }
        }
    }
}