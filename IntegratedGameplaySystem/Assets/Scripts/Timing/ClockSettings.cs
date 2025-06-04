using UnityEngine;

namespace IntegratedGameplaySystem
{
    [CreateAssetMenu(menuName = "ScriptableObjects/" + nameof(ClockSettings), fileName = "New " + nameof(ClockSettings))]
    public class ClockSettings : ScriptableObject
    {
        public float interval;
    }
}