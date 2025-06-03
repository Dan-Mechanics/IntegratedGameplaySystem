using UnityEngine;
using UnityEngine.UI;

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

        public static bool GetRandBool() 
        {
            return Random.value > 0.5f;
        }

        public static Text AddTextToCanvas(Transform canvas, GameObject textPrefab, Vector2 pos)
        {
            Transform transform = SpawnPrefab(textPrefab).transform;
            transform.SetParent(canvas);
            transform.localPosition = Vector3.zero;
            transform.GetComponent<RectTransform>().anchoredPosition = pos;

            Text txt = transform.GetComponent<Text>();
            txt.text = string.Empty;

            return txt;
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
    }
}