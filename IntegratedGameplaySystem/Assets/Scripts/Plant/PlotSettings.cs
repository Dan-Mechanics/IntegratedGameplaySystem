using System.Collections.Generic;
using UnityEngine;

namespace IntegratedGameplaySystem 
{
    [CreateAssetMenu(menuName = "ScriptableObjects/" + nameof(PlotSettings), fileName = "New " + nameof(PlotSettings))]
    public class PlotSettings : ScriptableObject
    {
        //public GameObject sprinklerButtonPrefab;
        //public GameObject grenadeButtonPrefab;

        public int width;
        public float spacing;

        //public int sprinklerCost;

        public UpgradeValues sprinkler;
        public UpgradeValues grenade;
    }
}