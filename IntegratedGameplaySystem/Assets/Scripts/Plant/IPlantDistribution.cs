using System.Collections.Generic;
using UnityEngine;

namespace IntegratedGameplaySystem
{
    /// <summary>
    /// I dont think i wanna have it in the inteffaces folder.
    /// </summary>
    public interface IPlantDistribution 
    {
        public void SpawnPlants(List<object> worldPlants);
    }
}