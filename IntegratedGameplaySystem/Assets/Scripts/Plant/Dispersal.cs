using System.Collections.Generic;
using UnityEngine;

namespace IntegratedGameplaySystem
{
    /// <summary>
    /// Does this count as strat pattern?
    /// </summary>
    public interface IPlantPlacementStrategy
    {
        Vector3 GetPlotPos(int index);
        void PlacePlants(Plant[] plants, int index);
        int GetPlantCount();
    }

    public class Dispersal : IPlantPlacementStrategy
    {
        private readonly DispersalSettings settings;

        public Dispersal(DispersalSettings settings)
        {
            this.settings = settings;
        }

        public int GetPlantCount()
        {
            return settings.plantCount;
        }

        public Vector3 GetPlotPos(int index)
        {
            return settings.offset + (2f * index * Vector3.right);
        }

        public void PlacePlants(Plant[] plants, int index)
        {
            for (int i = 0; i < plants.Length; i++)
            {
                plants[i].transform.position += Utils.GetRandomFlatPos(settings.dispersal);
                plants[i].transform.position += settings.offset;
                Utils.ApplyRandomRotation(plants[i].transform);
            }
        }
    }

    public class Plot : IPlantPlacementStrategy
    {
        private readonly PlotSettings settings;

        public Plot(PlotSettings settings)
        {
            this.settings = settings;
        }

        public int GetPlantCount()
        {
            return settings.width * settings.width;
        }

        public Vector3 GetPlotPos(int index)
        {
            return new Vector3(index * settings.width + settings.spacing / 2f, 0f, settings.spacing / 2f);
        }

        public void PlacePlants(Plant[] plants, int index)
        {
            Vector3 plotPos = GetPlotPos(index);

            int i;
            for (int x = 0; x < settings.width; x++)
            {
                for (int z = 0; z < settings.width; z++)
                {
                    i = x + (z * settings.width);

                    plants[i].transform.position += new Vector3(x * settings.spacing, 0f, z * settings.spacing) + plotPos;
                    //Utils.ApplyRandomRotation(plants[i].transform);
                }
            }
        }
    }
}