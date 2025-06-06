using UnityEngine;

namespace IntegratedGameplaySystem
{
    [CreateAssetMenu(menuName = "ScriptableObjects/" + nameof(DisplaySettings), fileName = "New " + nameof(DisplaySettings))]
    public class DisplaySettings : ScriptableObject
    {
        public GameObject canvas;

        public Sprite defaultSprite;
        public string defaultText;
        public GameObject text;
        public GameObject image;
        public Sprite pixel;
    }
}