using UnityEngine;

namespace IntegratedGameplaySystem
{
    [CreateAssetMenu(menuName = "ScriptableObjects/" + nameof(RunningShoesSettings), fileName = "New " + nameof(RunningShoesSettings))]
    public class RunningShoesSettings : ScriptableObject, IUpgradeValues, ISpeedSource
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

        public float runninShoesOnSpeed;

        public float GetSpeed() => runninShoesOnSpeed;
    }
}