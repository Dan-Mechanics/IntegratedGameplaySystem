using System;
using UnityEngine;

namespace IntegratedGameplaySystem
{
    public interface IUpgradeValues 
    {
        string Name { get; }
        int Cost { get; }
        bool SinglePurchase { get; }
        GameObject ButtonPrefab { get; }
        Color Color { get; }
        Vector3 Offset { get; }
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