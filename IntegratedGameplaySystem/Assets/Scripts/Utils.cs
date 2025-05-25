using UnityEngine;

namespace IntegratedGameplaySystem
{
    public static class Utils 
    {
        /// <summary>
        /// Useful for FNaF-like games.
        /// </summary>
        public static bool OneIn(int x)
        {
            return Random.Range(0, x + 1) == 0;
        }
    }
}