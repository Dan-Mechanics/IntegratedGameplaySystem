using UnityEngine;

namespace IntegratedGameplaySystem
{
    /// <summary>
    /// MaxStackSettings.cs ??
    /// </summary>
    [CreateAssetMenu(menuName = "ScriptableObjects/" + nameof(SingleItemHolderSettings), fileName = "New " + nameof(SingleItemHolderSettings))]
    public class SingleItemHolderSettings : ScriptableObject, IMaxStackSource
    {
        public int maxStack;

        public int GetMaxStack()
        {
            return maxStack;
        }
    }
}