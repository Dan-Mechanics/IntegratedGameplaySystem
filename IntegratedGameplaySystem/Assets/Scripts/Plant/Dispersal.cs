using UnityEngine;

namespace IntegratedGameplaySystem
{
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

        public void PlacePlants(PlantUnit[] plants)
        {
            for (int i = 0; i < plants.Length; i++)
            {
                plants[i].transform.position += Utils.GetRandomFlatPos(settings.dispersal);
                plants[i].transform.position += settings.offset;
                Utils.ApplyRandomRotation(plants[i].transform);
            }
        }
    }
}