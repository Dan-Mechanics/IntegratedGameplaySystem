using UnityEngine;
using UnityEngine.UI;

namespace IntegratedGameplaySystem
{
    /// <summary>
    /// Ik denk eerlijk dat het sneller is om mijn betterbutton gewoon om te schrijven dan this slow ass shit.
    /// </summary>
    public class SelfButton 
    {
        public void Setup(string msg) 
        {
            GameObject go = new GameObject("button");

            go.transform.SetParent(Object.FindObjectOfType<Canvas>().transform);
            go.transform.localPosition = Vector3.zero;
            go.transform.localEulerAngles = Vector3.zero;

            RectTransform rect = go.AddComponent<RectTransform>();
            rect.anchoredPosition = Vector2.zero;
            Image image = go.AddComponent<Image>();
            Button button = go.AddComponent<Button>();
            button.onClick.AddListener(OnClick);
            button.targetGraphic = image;
          //  button.navigation = Navigation.Mode.None;
            //button.transform.GetChild(0).GetComponent<Text>().text = msg;
        }

        public void OnClick() 
        {
            Debug.Log("click");
        }
    }
}