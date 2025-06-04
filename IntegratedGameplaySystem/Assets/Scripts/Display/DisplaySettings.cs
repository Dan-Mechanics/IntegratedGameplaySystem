using UnityEngine;

namespace IntegratedGameplaySystem
{
    [CreateAssetMenu(menuName = "ScriptableObjects/" + nameof(DisplaySettings), fileName = "New " + nameof(DisplaySettings))]
    public class DisplaySettings : ScriptableObject
    {
        public GameObject canvas;

        public Sprite holdingNothingSprite;
        public string hoveringNothingStr;
        public GameObject text;
        public GameObject image;
    }
}