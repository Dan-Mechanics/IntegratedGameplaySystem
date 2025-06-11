using UnityEngine;

namespace IntegratedGameplaySystem
{
    /// <summary>
    /// How many of the same item can the player hold?
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