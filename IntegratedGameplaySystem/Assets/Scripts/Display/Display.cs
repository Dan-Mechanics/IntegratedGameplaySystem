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
    /// 
    /// I thought about not using TMP because its more laggy i think.
    /// 
    /// NOTE: this class is NOT very solid.
    /// </summary>
    public class Display : IStartable, IDisposable
    {
        private const string NOT_HOVERING = " . . . ";
        
        private readonly MoneyCentral wallet;
        private readonly Interactor interactor;
        private readonly Clock tickClock;

        //public const string CANVAS_PREFAB_NAME = "canvas";
        private readonly Text hoveringText;
        private readonly Text moneyText;
        private readonly Text timerText;
        private readonly Image heldItemImage;

        public Display(Interactor interactor, MoneyCentral wallet, Clock tickClock)
        {
            this.wallet = wallet;
            this.interactor = interactor;
            this.tickClock = tickClock;

            var settings = ServiceLocator<IAssetService>.Locate().GetAssetWithType<DisplaySettings>();
            var canvas = Utils.SpawnPrefab(settings.canvas).transform;

            // !Clean ??

            float height = Utils.GetHeight(settings.text);

            hoveringText = Utils.AddTextToCanvas(canvas, settings.text, 1f * height * Vector2.down);
            timerText = Utils.AddTextToCanvas(canvas, settings.text, 2f * height * Vector2.down);
            moneyText = Utils.AddTextToCanvas(canvas, settings.text, 3f * height * Vector2.down);

            heldItemImage = Utils.AddImageToCanvas(canvas, settings.image, Vector2.up * 15f);

            Utils.SnapToBottom(heldItemImage.GetComponent<RectTransform>());

            heldItemImage.sprite = ServiceLocator<IAssetService>.Locate().GetAssetWithType<PlantBlueprint>().sprite;
        }

        public void Start()
        {
            wallet.OnMoneyChanged += UpdateMoneyText;
            interactor.OnHoverChange += UpdateHoveringText;
            tickClock.OnNewTime += UpdateTimerText;
        }

        public void UpdateHoveringText(Transform hit) => hoveringText.text = hit ? hit.name : NOT_HOVERING;
        //public void UpdateMoneyText(int money, int maxMoney) => moneyText.text = money > 0 ? $"({money} / {maxMoney})" : string.Empty;
        public void UpdateMoneyText(int money, int maxMoney) => moneyText.text = $"({money} / {maxMoney})";
        public void UpdateTimerText(float time) => timerText.text = time.ToString();

        public void Dispose()
        {
            wallet.OnMoneyChanged -= UpdateMoneyText;
            interactor.OnHoverChange -= UpdateHoveringText;
            tickClock.OnNewTime -= UpdateTimerText;
        }
    }
}