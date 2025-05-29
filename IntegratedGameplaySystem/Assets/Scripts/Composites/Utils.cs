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
            GameObject go = Object.Instantiate(prefab, prefab.transform.position, prefab.transform.rotation);
            go.name = prefab.name;
            return go;
        }

        public static Vector3 GetRandomFlatPos(float spread, float y = 0f)
        {
            Vector2 rand = Random.insideUnitCircle * spread;
            return new Vector3(rand.x, y, rand.y);
        }

        public static T StringToEnum<T>(string str) 
        {
            return (T)System.Enum.Parse(typeof(T), str);
        }

        /*[System.Obsolete]
        public static GameObject LoadPrefab(string name) 
        {
            GameObject prefab = Resources.Load<GameObject>($"Prefabs/{name}");

            // ????
            if (prefab == null)
                throw new System.Exception("Can't find that prefab!");

            return prefab;
        }

        [System.Obsolete]
        public static T LoadData<T>(string name) where T : Object
        {
            T data = Resources.Load<T>($"Data/{name}");

            // ????
            if (data == null)
                throw new System.Exception("Can't find that data!");

            return data;
        }*/
    }
}