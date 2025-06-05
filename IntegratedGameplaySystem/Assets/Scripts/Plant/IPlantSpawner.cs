using System.Collections.Generic;
using UnityEngine;

namespace IntegratedGameplaySystem
{
    /// <summary>
    /// I dont think i wanna have it in the inteffaces folder.
    /// </summary>
    public interface IPlantSpawner 
    {
        public void Spawn(List<object> output, PlantBlueprint blueprint, Vector3 offset);
    }
}