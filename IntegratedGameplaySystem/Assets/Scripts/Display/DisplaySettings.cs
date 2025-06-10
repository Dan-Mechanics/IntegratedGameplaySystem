using UnityEngine;

namespace IntegratedGameplaySystem
{
    /// <summary>
    /// Mainly passes important references to prefabs and defaults.
    /// </summary>
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