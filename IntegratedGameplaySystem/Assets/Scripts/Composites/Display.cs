using UnityEngine;
using UnityEngine.UI;

namespace IntegratedGameplaySystem
{
    /// <summary>
    /// I want this class to pipe info to all UI shit.
    /// This class might need a reference to the canvas? or like 
    /// it makes shit in the canvas itslef but makign UI code is kinda annoying
    /// so i would rahter have us not do that right.
    /// Maybe if we have some clever solution for assets??
    /// Where like we just spawn it in and it works??
    /// </summary>
    public class Display
    {
        public const string CANVAS_PREFAB_NAME = "canvas";
        private readonly Text hoveringText;
        private readonly Text moneyText;

        public Display(GameObject prefab)
        {
            Transform canvas = Utils.SpawnPrefab(prefab).transform;
            hoveringText = canvas.GetChild(0).GetComponent<Text>();
            moneyText = canvas.GetChild(1).GetComponent<Text>();
        }

        public void UpdateHoveringText(Transform hit) => hoveringText.text = hit ? hit.name : string.Empty;
        public void UpdateMoneyText(int money, int maxMoney) => moneyText.text = money > 0 ? $"({money} / {maxMoney})" : string.Empty;
    }
}