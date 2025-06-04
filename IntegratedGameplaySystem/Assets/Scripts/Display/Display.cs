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
        private readonly Inventory holdingHandler;

        public DisplaySettings Settings { get; private set; }
        public Text HoveringText { get; private set; }
        public Text MoneyText { get; private set; }
        public Text TimerText { get; private set; }
        public Image HeldItemImage { get; private set; }
        public Text ItemCountText { get; private set; }

        private Display(Interactor interactor, MoneyCentral wallet, Clock tickClock, Inventory holdingHandler)
        {
            this.wallet = wallet;
            this.interactor = interactor;
            this.tickClock = tickClock;
            this.holdingHandler = holdingHandler;
        }

        public static Display CreateAndInitializeUI(Interactor interactor, MoneyCentral wallet, Clock tickClock, Inventory holdingHandler) 
        {
            Display display = new Display(interactor, wallet, tickClock, holdingHandler);

            display.Settings = ServiceLocator<IAssetService>.Locate().GetAssetWithType<DisplaySettings>();
            var canvas = Utils.SpawnPrefab(display.Settings.canvas).transform;

            // !Clean ??

            float height = Utils.GetHeight(display.Settings.text);

            display.HoveringText = Utils.AddTextToCanvas(canvas, display.Settings.text, 1f * height * Vector2.down);
            display.TimerText = Utils.AddTextToCanvas(canvas, display.Settings.text, 2f * height * Vector2.down);
            display.MoneyText = Utils.AddTextToCanvas(canvas, display.Settings.text, 3f * height * Vector2.down);
            
            display.HeldItemImage = Utils.AddImageToCanvas(canvas, display.Settings.image, Vector2.up * 15f);
            display.ItemCountText = Utils.AddTextToCanvas(canvas, display.Settings.text, (15f + display.HeldItemImage.rectTransform.sizeDelta.y / 2f) * (Vector2.up+Vector2.left));
            display.ItemCountText.color = Color.black;
            Utils.SnapToBottom(display.HeldItemImage.rectTransform);
            Utils.SnapToBottom(display.ItemCountText.rectTransform);
            //display.HeldItemImage.sprite = ServiceLocator<IAssetService>.Locate().GetAssetWithType<PlantBlueprint>().sprite;

            return display;
        }

        public void Start()
        {
            wallet.OnMoneyChanged += UpdateMoneyText;
            interactor.OnHoverChange += UpdateHoveringText;
            tickClock.OnNewTime += UpdateTimerText;
            holdingHandler.OnHold += UpdateItem;
            holdingHandler.OnCountChange += UpdateItemCount;
        }

        public void UpdateHoveringText(IHoverable hover) => HoveringText.text = hover != null ? hover.Name : NOT_HOVERING;
        public void UpdateMoneyText(int money, int maxMoney) => MoneyText.text = $"({money} / {maxMoney})";
        public void UpdateTimerText(float time) => TimerText.text = time.ToString();
        public void UpdateItem(IItem item) => HeldItemImage.sprite = item == null ? Settings.holdingNothingSprite : item.Sprite;
        public void UpdateItemCount(int count) => ItemCountText.text = count.ToString();

        public void Dispose()
        {
            wallet.OnMoneyChanged -= UpdateMoneyText;
            interactor.OnHoverChange -= UpdateHoveringText;
            tickClock.OnNewTime -= UpdateTimerText;
        }
    }
}