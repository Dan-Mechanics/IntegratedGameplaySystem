using UnityEngine;
using UnityEngine.UI;

namespace IntegratedGameplaySystem
{
    public static class Utils 
    {
        /// <summary>
        /// Useful for FNaF-like games.
        /// </summary>
        public static bool OneIn(int x)
        {
            return Random.Range(0, x) == 0;
        }

        public static bool GetRandBool() 
        {
            return Random.value > 0.5f;
        }

        public static Text MakeText(Transform canvas, GameObject textPrefab, Vector2 pos)
        {
            Transform trans = Utils.SpawnPrefab(textPrefab).transform;
            trans.SetParent(canvas);
            trans.localPosition = Vector3.zero;
            trans.GetComponent<RectTransform>().anchoredPosition = pos;
            Text txt = trans.GetComponent<Text>();
            txt.text = string.Empty;

            return txt;
        }

        public static void ApplyRandomRotation(Transform trans) 
        {
            trans.Rotate(Vector3.up * Random.Range(0f, 360f), Space.Self);
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