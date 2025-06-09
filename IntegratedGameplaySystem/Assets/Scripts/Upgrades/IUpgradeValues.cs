using System;
using UnityEngine;

namespace IntegratedGameplaySystem
{
    public interface IUpgradeValues 
    {
        public string Name { get; }
        public int Cost { get; }
        public bool SinglePurchase { get; }
        public GameObject ButtonPrefab { get; }
        public Color Color { get; }
        public Vector3 Offset { get; }
    }
    
    /*[Serializable]
    public class UpgradeValues
    {
        public string name;
        public int cost;
        public bool singlePurchase;
        public GameObject buttonPrefab;

        // Mateiral??
    }*/
}