using UnityEngine;

namespace IntegratedGameplaySystem
{
    public class Plot : PlantDistribution, IPlantPlacementStrategy
    {
        private readonly PlotSettings settings;
        private int index;

        public Plot(PlotSettings settings)
        {
            this.settings = settings;
        }

        public void SetPlotIndex(int index) => this.index = index;

        public override Vector3 GetWorldCenter() 
        {
            Vector3 center = GetWorldPosition();
            center += 0.5f * settings.spacing * settings.width * Vector3.forward;
            center -= settings.spacing / 2f * Vector3.forward;
            center += 0.5f * settings.spacing * settings.width * Vector3.right;
            center -= settings.spacing / 2f * Vector3.right;
            return center;
        }

        public override int GetPlantCount()
        {
            return settings.width * settings.width;
        }

        public override Vector3 GetWorldPosition()
        {
            return new Vector3(index * settings.width + settings.spacing / 2f, 0f, settings.spacing / 2f);
        }

        public void PlacePlants(PlantUnit[] plants)
        {
            Vector3 plotPos = GetWorldPosition();

            int index;
            for (int x = 0; x < settings.width; x++)
            {
                for (int z = 0; z < settings.width; z++)
                {
                    index = x + (z * settings.width);
                    plants[index].transform.position += new Vector3(x * settings.spacing, 0f, z * settings.spacing) + plotPos;
                }
            }
        }
    }
}