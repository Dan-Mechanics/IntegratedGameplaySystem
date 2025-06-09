using UnityEngine;

namespace IntegratedGameplaySystem
{
    /// <summary>
    /// MaxStackSettings.cs ??
    /// </summary>
    [CreateAssetMenu(menuName = "ScriptableObjects/" + nameof(HandSettings), fileName = "New " + nameof(HandSettings))]
    public class HandSettings : ScriptableObject, IMaxStackSource
    {
        public int maxStack;

        public int GetMaxStack()
        {
            return maxStack;
        }
    }
}