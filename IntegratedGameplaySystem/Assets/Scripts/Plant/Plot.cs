using System.Collections.Generic;
using UnityEngine;

namespace IntegratedGameplaySystem
{
    /// <summary>
    /// I dont think i wanna have it in the inteffaces folder.
    /// </summary>
    public interface IPlantSpawner 
    {
        public void Spawn(List<object> components, PlantBlueprint blueprint, Vector3 offset);
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

        public void Spawn(List<object> components, PlantBlueprint blueprint, Vector3 offset)
        {
            Plant temp;
            
            for (int i = 0; i < plantCount; i++)
            {
                temp = new Plant(blueprint);

                temp.gameObject.transform.position += Utils.GetRandomFlatPos(dispersal);
                temp.gameObject.transform.position += offset;
                Utils.ApplyRandomRotation(temp.gameObject.transform);

                components.Add(temp);
            }
        }
    }

    public class Plot : IPlantSpawner
    {
        public readonly int width;
        public readonly float spacing;

        /// <summary>
        /// Consider moving to a settigns scriptableobject asset . 
        /// </summary>
        public Plot(int width, float spacing)
        {
            this.width = width;
            this.spacing = spacing;
        }

        public void Spawn(List<object> components, PlantBlueprint blueprint, Vector3 offset)
        {
            Plant temp;

            for (int x = 0; x < width; x++)
            {
                for (int z = 0; z < width; z++)
                {
                    temp = new Plant(blueprint);

                    temp.gameObject.transform.position += new Vector3(x * spacing, 0f, z * spacing);
                    temp.gameObject.transform.position += offset;
                    // Utils.ApplyRandomRotation(temp.gameObject.transform);

                    components.Add(temp);
                }
            }
        }
    }
}