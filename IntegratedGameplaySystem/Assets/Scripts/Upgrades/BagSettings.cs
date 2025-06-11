using UnityEngine;

namespace IntegratedGameplaySystem
{
    [CreateAssetMenu(menuName = "ScriptableObjects/" + nameof(BagSettings), fileName = "New " + nameof(BagSettings))]
    public class BagSettings : ScriptableObject, IUpgradeValues, IMaxStackSource
    {
        public string Name => name;
        public int Cost => cost;
        public bool SinglePurchase => singlePurchase;
        public GameObject ButtonPrefab => buttonPrefab;
        public Color Color => color;
        public Vector3 Offset => offset;

        public int cost;
        public bool singlePurchase;

        public GameObject buttonPrefab;
        public Color color;
        public Vector3 offset;

        public int bagMaxItems;

        public int GetMaxStack() => bagMaxItems;
    }
}