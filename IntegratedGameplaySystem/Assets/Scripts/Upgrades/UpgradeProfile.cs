using System;
using UnityEngine;

namespace IntegratedGameplaySystem
{
    [Serializable]
    public class UpgradeProfile
    {
        public string name;
        public int cost;
        public bool singlePurchase;
        //public Vector3 position;
        // becuase we might want to have some uniqueness.
        public GameObject buttonPrefab;
    }
}