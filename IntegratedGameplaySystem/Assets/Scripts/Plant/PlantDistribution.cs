using UnityEngine;

namespace IntegratedGameplaySystem
{
    public abstract class PlantDistribution
    {
        public abstract Vector3 GetWorldPosition();
        public abstract int GetPlantCount();
        public abstract Vector3 GetWorldCenter();
    }
}