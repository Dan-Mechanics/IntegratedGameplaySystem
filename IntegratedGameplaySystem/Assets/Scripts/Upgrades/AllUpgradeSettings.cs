using System.Collections.Generic;
using UnityEngine;

namespace IntegratedGameplaySystem 
{
    [CreateAssetMenu(menuName = "ScriptableObjects/" + nameof(AllUpgradeSettings), fileName = "New " + nameof(AllUpgradeSettings))]
    public class AllUpgradeSettings : ScriptableObject
    {
        public UpgradeValues sprinkler;
        public UpgradeValues grenade;
        public UpgradeValues runningShoes;
        public UpgradeValues bigHands;

        public AOE area;
        public float grenadeHeight;
        public float runningShoesBoost;
        public GameObject sprinklerEffect;
        public GameObject grenadeEffect;
    }
}