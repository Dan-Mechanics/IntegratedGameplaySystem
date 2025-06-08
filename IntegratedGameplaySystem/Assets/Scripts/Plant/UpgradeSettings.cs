using System.Collections.Generic;
using UnityEngine;

namespace IntegratedGameplaySystem 
{
    [CreateAssetMenu(menuName = "ScriptableObjects/" + nameof(UpgradeSettings), fileName = "New " + nameof(UpgradeSettings))]
    public class UpgradeSettings : ScriptableObject
    {
        public UpgradeProfile sprinkler;
        public UpgradeProfile grenade;

        public float range;
        public LayerMask mask;

        //public GameObject buttonPrefab;
        public GameObject sprinklerEffect;
        public GameObject grenadeEffect;
    }
}