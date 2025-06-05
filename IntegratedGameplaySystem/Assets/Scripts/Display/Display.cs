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
    /// 
    /// 
    /// I think this clasess is allowed to have hella references and YES i can make it better.
    /// </summary>
    public class Display : IStartable, IDisposable
    {
        private readonly MoneyCentral moneyCentral;
        private readonly Interactor interactor;
        private readonly IChangeTracker<float> tickClock;
        private readonly Hand inventory;

        // FIX !!!
        private DisplaySettings settings;
        private Text hoveringText;
        private Text moneyText;
        private Text timerText;
        private Image heldItemImage;
        private Text heldItemCountText;

        private Display(Interactor interactor, MoneyCentral moneyCentral, IChangeTracker<float> tickClock, Hand inventory)
        {
            this.moneyCentral = moneyCentral;
            this.interactor = interactor;
            this.tickClock = tickClock;
            this.inventory = inventory;
        }

        /// <summary>
        /// Factory.
        /// </summary>
        public static Display CreateAndInitializeUI(Interactor interactor, MoneyCentral moneyCentral, IChangeTracker<float> tickClock, Hand inventory) 
        {
            Display display = new Display(interactor, moneyCentral, tickClock, inventory);

            display.settings = ServiceLocator<IAssetService>.Locate().GetAssetWithType<DisplaySettings>();
            var canvas = Utils.SpawnPrefab(display.settings.canvas).transform;

            // !Clean ??

            float height = Utils.GetHeight(display.settings.text);

            display.hoveringText = Utils.AddTextToCanvas(canvas, display.settings.text, 1f * height * Vector2.down);
            display.timerText = Utils.AddTextToCanvas(canvas, display.settings.text, 2f * height * Vector2.down);
            display.moneyText = Utils.AddTextToCanvas(canvas, display.settings.text, 3f * height * Vector2.down);

            display.heldItemImage = Utils.AddImageToCanvas(canvas, display.settings.image, Vector2.up * 15f);
            display.heldItemCountText = Utils.AddTextToCanvas(canvas, display.settings.text, (15f + display.heldItemImage.rectTransform.sizeDelta.y / 2f) * (Vector2.up+Vector2.left));
            display.heldItemCountText.color = Color.black;
            Utils.SnapToBottom(display.heldItemImage.rectTransform);
            Utils.SnapToBottom(display.heldItemCountText.rectTransform);

            Image overlay = Utils.AddImageToCanvas(canvas, display.settings.image, Vector2.up * 15f);
            overlay.sprite  = display.settings.holdingNothingSprite;
            Utils.SnapToBottom(overlay.rectTransform);

            return display;
        }

        public void Start()
        {
            moneyCentral.OnMoneyChanged += UpdateMoneyText;
            interactor.OnHoverChange    += UpdateHoveringText;
            tickClock.OnChange         += UpdateTimerText;
            inventory.OnItemChange      += UpdateItem;
            inventory.OnCountChange     += UpdateItemCount;
            inventory.AtMaxCapacity     += UpdateMaxCapacity;
        }

        public void UpdateHoveringText(string hover) => hoveringText.text = string.IsNullOrEmpty(hover) ? settings.hoveringNothingText : hover;
        public void UpdateMoneyText(int money, int maxMoney) => moneyText.text = $"({money} / {maxMoney})";
        public void UpdateTimerText(float time) => timerText.text = time.ToString();
        public void UpdateItem(IItemArchitype item) => heldItemImage.sprite = item == null ? settings.holdingNothingSprite : item.Sprite;
        public void UpdateItemCount(int count) => heldItemCountText.text = count.ToString();
        public void UpdateMaxCapacity(bool atCapacity) => heldItemCountText.color = atCapacity ? Color.red : Color.black;

        public void Dispose()
        {
            moneyCentral.OnMoneyChanged -= UpdateMoneyText;
            interactor.OnHoverChange    -= UpdateHoveringText;
            tickClock.OnChange -= UpdateTimerText;
            inventory.OnItemChange      -= UpdateItem;
            inventory.OnCountChange     -= UpdateItemCount;
            inventory.AtMaxCapacity     -= UpdateMaxCapacity;
        }
    }
}