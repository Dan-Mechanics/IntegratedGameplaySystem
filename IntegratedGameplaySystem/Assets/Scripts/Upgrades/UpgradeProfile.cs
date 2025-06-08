using System;
using UnityEngine;

namespace IntegratedGameplaySystem
{
    [System.Serializable]
    public class UpgradeProfile
    {
        public string name;
        public int cost;
        public bool singlePurchase;
        public GameObject buttonPrefab;
    }
}