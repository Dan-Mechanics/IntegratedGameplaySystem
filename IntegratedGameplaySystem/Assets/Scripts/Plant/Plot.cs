using System.Collections.Generic;
using UnityEngine;

namespace IntegratedGameplaySystem
{
    /// <summary>
    /// Does this count as strat pattern?
    /// </summary>
    public interface IPlantDistribution
    {
        Vector3 GetWorldPosition();
        void PlacePlants(SoilUnit[] plants);
        int GetPlantCount();
        Vector3 GetWorldCenter();
    }

    public class Dispersal : IPlantDistribution
    {
        private readonly DispersalSettings settings;

        public Dispersal(DispersalSettings settings)
        {
            this.settings = settings;
        }

        public Vector3 GetWorldCenter()
        {
            return GetWorldPosition();
        }

        public int GetPlantCount()
        {
            return settings.plantCount;
        }

        public Vector3 GetWorldPosition()
        {
            return settings.offset;
        }

        public void PlacePlants(SoilUnit[] plants)
        {
            for (int i = 0; i < plants.Length; i++)
            {
                plants[i].transform.position += Utils.GetRandomFlatPos(settings.dispersal);
                plants[i].transform.position += settings.offset;
                Utils.ApplyRandomRotation(plants[i].transform);
            }
        }
    }

    public class Plot : IPlantDistribution
    {
        private readonly PlotSettings settings;
        private int index;

        public Plot(PlotSettings settings)
        {
            this.settings = settings;
        }

        public void SetPlotIndex(int index) => this.index = index;

        public Vector3 GetWorldCenter() 
        {
            Vector3 center = GetWorldPosition();
            center += 0.5f * settings.spacing * settings.width * Vector3.forward;
            center -= (settings.spacing / 2f) * Vector3.forward;
            center += 0.5f * settings.spacing * settings.width * Vector3.right;
            center -= (settings.spacing / 2f) * Vector3.right;
            return center;
        }

        public int GetPlantCount()
        {
            return settings.width * settings.width;
        }

        public Vector3 GetWorldPosition()
        {
            return new Vector3(index * settings.width + settings.spacing / 2f, 0f, settings.spacing / 2f);
        }

        public void PlacePlants(SoilUnit[] plants)
        {
            Vector3 plotPos = GetWorldPosition();

            int index;
            for (int x = 0; x < settings.width; x++)
            {
                for (int z = 0; z < settings.width; z++)
                {
                    index = x + (z * settings.width);

                    plants[index].transform.position += new Vector3(x * settings.spacing, 0f, z * settings.spacing) + plotPos;
                    //Utils.ApplyRandomRotation(plants[i].transform);
                }
            }
        }
    }
}