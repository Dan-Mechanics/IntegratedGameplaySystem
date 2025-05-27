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

        public static GameObject SpawnPrefab(GameObject prefab) 
        {
            return Object.Instantiate(prefab, prefab.transform.position, prefab.transform.rotation);
        }

        public static GameObject LoadPrefab(string name) 
        {
            GameObject prefab = Resources.Load<GameObject>($"Prefabs/{name}");

            // ????
            if (!prefab)
                throw new System.Exception("Can't find that prefab!");

            return prefab;
        }
    }
}