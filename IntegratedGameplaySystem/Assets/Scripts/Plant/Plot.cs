using System.Collections.Generic;
using UnityEngine;

namespace IntegratedGameplaySystem
{
    /// <summary>
    /// Does this count as strat pattern?
    /// </summary>
    public abstract class PlantDistribution
    {
        public abstract Vector3 GetWorldPosition();
        public abstract int GetPlantCount();
        public abstract Vector3 GetWorldCenter();
    }

    public interface IPlantPlacementStrategy 
    {
        void PlacePlants(SoilUnit[] units);
    }

    public class Dispersal : PlantDistribution, IPlantPlacementStrategy
    {
        private readonly DispersalSettings settings;

        public Dispersal(DispersalSettings settings)
        {
            this.settings = settings;
        }

        public override Vector3 GetWorldCenter()
        {
            return GetWorldPosition();
        }

        public override int GetPlantCount()
        {
            return settings.plantCount;
        }

        public override Vector3 GetWorldPosition()
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
            center -= (settings.spacing / 2f) * Vector3.forward;
            center += 0.5f * settings.spacing * settings.width * Vector3.right;
            center -= (settings.spacing / 2f) * Vector3.right;
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