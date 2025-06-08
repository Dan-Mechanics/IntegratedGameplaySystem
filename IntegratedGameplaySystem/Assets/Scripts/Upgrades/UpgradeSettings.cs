using System.Collections.Generic;
using UnityEngine;

namespace IntegratedGameplaySystem 
{
    [CreateAssetMenu(menuName = "ScriptableObjects/" + nameof(UpgradeSettings), fileName = "New " + nameof(UpgradeSettings))]
    public class UpgradeSettings : ScriptableObject
    {
        public UpgradeProfile sprinkler;
        public UpgradeProfile grenade;
        public UpgradeProfile runningShoes;
        public UpgradeProfile bigHands;

        public AOE area;
        public float grenadeHeight;
        public float runningShoesBoost;
        public GameObject sprinklerEffect;
        public GameObject grenadeEffect;
    }
}