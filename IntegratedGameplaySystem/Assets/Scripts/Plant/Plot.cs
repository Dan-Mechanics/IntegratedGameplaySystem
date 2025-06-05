using System.Collections.Generic;
using UnityEngine;

namespace IntegratedGameplaySystem 
{ 
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

        public void Spawn(List<object> output, PlantFlyweight blueprint, Vector3 offset)
        {
            Plant temp;

            for (int x = 0; x < width; x++)
            {
                for (int z = 0; z < width; z++)
                {
                    temp = new Plant(blueprint);

                    temp.transform.position += new Vector3(x * spacing, 0f, z * spacing);
                    temp.transform.position += offset;
                    // Utils.ApplyRandomRotation(temp.gameObject.transform);

                    output.Add(temp);
                }
            }
        }
    }
}