using UnityEngine;

namespace IntegratedGameplaySystem
{
    /// <summary>
    /// Mainly passes important references to prefabs and defaults.
    /// </summary>
    [CreateAssetMenu(menuName = "ScriptableObjects/" + nameof(DisplaySettings), fileName = "New " + nameof(DisplaySettings))]
    public class DisplaySettings : ScriptableObject
    {
        public GameObject canvasPrefab;
        public GameObject textPrefab;
        public GameObject imagePrefab;

        public Sprite defaultSprite;
        public string defaultText;
        public Sprite pixel;
    }
}