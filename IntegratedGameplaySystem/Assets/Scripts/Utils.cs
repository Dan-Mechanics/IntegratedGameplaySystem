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

        /*public static float GetHeight(GameObject go) 
        {
            return go.GetComponent<RectTransform>().sizeDelta.y;
        }

        public static float GetWidth(GameObject go)
        {
            return go.GetComponent<RectTransform>().sizeDelta.x;
        }

        public static void SnapToLeft(RectTransform rect) 
        {
            rect.pivot = new Vector2(0f, 0.5f);
            Snap(rect);
        }

        public static void SnapToRight(RectTransform rect)
        {
            rect.pivot = new Vector2(1f, 0.5f);
            Snap(rect);
        }

        public static void SnapToBottom(RectTransform rect)
        {
            rect.pivot = new Vector2(0.5f, 0f);
            Snap(rect);
        }

        public static void SnapToTop(RectTransform rect)
        {
            rect.pivot = new Vector2(0.5f, 1f);
            Snap(rect);
        }

        public static void SnapToCenter(RectTransform rect)
        {
            rect.pivot = new Vector2(0.5f, 0.5f);
            Snap(rect);
        }

        private static void Snap(RectTransform rect)
        {
            rect.anchorMin = rect.pivot;
            rect.anchorMax = rect.pivot;
            //rect.anchoredPosition = Vector2.up * 15f;
        }*/

        public static Text AddTextToCanvas(Transform canvas, GameObject textPrefab)
        {
            Transform transform = SpawnPrefab(textPrefab).transform;
            transform.SetParent(canvas);
            transform.localPosition = Vector3.zero;
            //transform.GetComponent<RectTransform>().anchoredPosition = pos;

            Text txt = transform.GetComponent<Text>();
            txt.text = string.Empty;

            return txt;
        }

        public static Image AddImageToCanvas(Transform canvas, GameObject imagePrefab)
        {
            Transform transform = SpawnPrefab(imagePrefab).transform;
            transform.SetParent(canvas);
            transform.localPosition = Vector3.zero;
            //transform.GetComponent<RectTransform>().anchoredPosition = pos;

            Image img = transform.GetComponent<Image>();
            img.sprite = null;

            return img;
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