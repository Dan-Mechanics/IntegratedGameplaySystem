using UnityEngine;

namespace IntegratedGameplaySystem
{
    [CreateAssetMenu(menuName = "ScriptableObjects/" + nameof(DispersalSettings), fileName = "New " + nameof(DispersalSettings))]
    public class DispersalSettings : ScriptableObject
    {
        public int plantCount;
        public float dispersal;
        public Vector3 offset;
    }
}