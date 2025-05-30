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
    public class Display : IStartable, IDisposable
    {
        private readonly Wallet wallet;
        private readonly Interactor interactor;
        
        //public const string CANVAS_PREFAB_NAME = "canvas";
        private readonly Text hoveringText;
        private readonly Text moneyText;

        public Display(Interactor interactor, Wallet wallet)
        {
            this.wallet = wallet;
            this.interactor = interactor;

            DisplaySettings settings = ServiceLocator<IAssetService>.Locate().GetByType<DisplaySettings>();
            Transform canvas = Utils.SpawnPrefab(settings.canvas).transform;

            // make something for this.
            hoveringText = MakeText(canvas, settings.text, 0.5f * -30f * Vector2.up);
            moneyText = MakeText(canvas, settings.text, 1.5f * -30f * Vector2.up);
        }

        public void Start()
        {
            wallet.OnMoneyChanged += UpdateMoneyText;
            interactor.OnHoverChange += UpdateHoveringText;

            //EventManagerGeneric<int>.RaiseEvent(Occasion.EARN_MONEY, 10);
        }

        private Text MakeText(Transform canvas, GameObject textPrefab, Vector2 pos) 
        {
            Transform trans = Utils.SpawnPrefab(textPrefab).transform;
            trans.SetParent(canvas);
            trans.localPosition = Vector3.zero;
            trans.GetComponent<RectTransform>().anchoredPosition = pos;
            Text txt = trans.GetComponent<Text>();
            txt.text = string.Empty;

            return txt;
        }

        public void UpdateHoveringText(Transform hit) => hoveringText.text = hit ? hit.name : string.Empty;
        public void UpdateMoneyText(int money, int maxMoney) => moneyText.text = money > 0 ? $"({money} / {maxMoney})" : string.Empty;

        public void Dispose()
        {
            wallet.OnMoneyChanged -= UpdateMoneyText;
            interactor.OnHoverChange -= UpdateHoveringText;
        }
    }
}