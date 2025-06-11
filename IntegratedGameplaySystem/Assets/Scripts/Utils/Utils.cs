using UnityEngine;

namespace IntegratedGameplaySystem
{
    public static class Utils 
    {
        /// <summary>
        /// A one in X chance.
        /// </summary>
        public static bool OneIn(int x)
        {
            return Random.Range(0, x) == 0;
        }
        
        public static bool RandomWithPercentage(float p) 
        {
            if (p <= 0f)
                return false;

            if (p >= 100f)
                return true;
            
            return Random.Range(0f, 100f) <= p;
        }

        public static bool GetRandBool()
        {
            return Random.value > 0.5f;
        }

        public static void ApplyRandomRotation(Transform transform) 
        {
            transform.Rotate(Vector3.up * Random.Range(0f, 360f), Space.Self);
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

        public static bool IsStringValid(string str) 
        {
            return !string.IsNullOrEmpty(str) && !string.IsNullOrWhiteSpace(str);
        }
    }
}