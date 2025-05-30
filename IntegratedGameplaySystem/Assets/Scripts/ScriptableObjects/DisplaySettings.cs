using UnityEngine;

namespace IntegratedGameplaySystem
{
    [CreateAssetMenu(menuName = "ScriptableObjects/" + nameof(DisplaySettings), fileName = "New " + nameof(DisplaySettings))]
    public class DisplaySettings : ScriptableObject
    {
        public GameObject canvas;
        public GameObject text;
    }
}