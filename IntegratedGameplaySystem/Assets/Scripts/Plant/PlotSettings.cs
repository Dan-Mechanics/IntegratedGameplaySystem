using System.Collections.Generic;
using UnityEngine;

namespace IntegratedGameplaySystem 
{
    [CreateAssetMenu(menuName = "ScriptableObjects/" + nameof(PlotSettings), fileName = "New " + nameof(PlotSettings))]
    public class PlotSettings : ScriptableObject
    {
        //public PlantFlyweight plant;
        public int width;
        public float spacing;
    }
}