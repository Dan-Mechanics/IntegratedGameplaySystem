using System.Collections.Generic;
using UnityEngine;

namespace IntegratedGameplaySystem 
{
    [CreateAssetMenu(menuName = "ScriptableObjects/" + nameof(GrenadeSettings), fileName = "New " + nameof(GrenadeSettings))]
    public class GrenadeSettings : ScriptableObject, IUpgradeValues
    {
        public string Name => name;
        public int Cost => cost;
        public bool SinglePurchase => singlePurchase;
        public GameObject ButtonPrefab => buttonPrefab;
        public Color Color => color;
        public Vector3 Offset => offset;

        public int cost;
        public bool singlePurchase;

        /// <summary>
        /// Is this needed because we have the color thing ??
        /// </summary>
        public GameObject buttonPrefab;
        public Color color;
        public Vector3 offset;

        public AOE area;
        public GameObject grenadeEffect;
    }
}