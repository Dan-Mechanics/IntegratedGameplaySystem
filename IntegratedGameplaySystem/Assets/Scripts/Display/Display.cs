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
        private readonly Hand hand;

        // FIX !!!
        private DisplaySettings settings;

        private Text hovering;
        private Text money;
        private Text timer;
        private Image heldItem;
        private Text itemCount;

        public Display(Interactor interactor, MoneyCentral moneyCentral, IChangeTracker<float> tickClock, Hand hand)
        {
            this.moneyCentral = moneyCentral;
            this.interactor = interactor;
            this.tickClock = tickClock;
            this.hand = hand;

            Setup();
        }

        /// <summary>
        /// Factory.
        /// Consider making this a builder and then making it proper ??
        /// </summary>
        private void Setup() 
        {
            settings = ServiceLocator<IAssetService>.Locate().GetAssetWithType<DisplaySettings>();
            Transform canvas = Utils.SpawnPrefab(settings.canvas).transform;


            // Amazing code here:

            hovering = Utils.AddToCanvas<Text>(canvas, settings.text);
            EasyUI temp = new EasyUI(hovering.rectTransform);
            temp.SnapTo(Image.Origin180.Bottom, 1f * temp.GetHeight() * Vector2.down);

            timer = Utils.AddToCanvas<Text>(canvas, settings.text);
            temp.Set(timer.rectTransform);
            temp.SnapTo(Image.Origin180.Bottom, 2f * temp.GetHeight() * Vector2.down);

            money = Utils.AddToCanvas<Text>(canvas, settings.text);
            temp.Set(money.rectTransform);
            temp.SnapTo(Image.Origin180.Bottom, 3f * temp.GetHeight() * Vector2.down);

            heldItem = Utils.AddToCanvas<Image>(canvas, settings.image);
            temp.Set(heldItem.rectTransform);
            temp.SnapTo(Image.Origin180.Bottom, Vector2.up * 15f);

            Vector2 itemCountOffset = (15f + temp.GetHeight() / 2f) * (Vector2.up + Vector2.left);

            itemCount = Utils.AddToCanvas<Text>(canvas, settings.text);
            itemCount.color = Color.black;
            temp.Set(itemCount.rectTransform);
            temp.SnapTo(Image.Origin180.Bottom, itemCountOffset);

            Image overlay = Utils.AddToCanvas<Image>(canvas, settings.image);
            overlay.sprite = settings.holdingNothingSprite;
            temp.Set(overlay.rectTransform);
            temp.SnapTo(Image.Origin180.Bottom, Vector2.up * 15f);
        }

        public void Start()
        {
            moneyCentral.OnMoneyChanged += UpdateMoneyText;
            interactor.OnHoverChange    += UpdateHoveringText;
            tickClock.OnChange         += UpdateTimerText;
            hand.OnItemChange      += UpdateItem;
            hand.OnCountChange     += UpdateItemCount;
            hand.AtMaxCapacity     += UpdateMaxCapacity;
        }

        /// <summary>
        /// T1 --> T2
        /// </summary>
        public class DataChannel<T1, T2> : IDisposable
        {
            public IChangeTracker<T1> changeTracker;
            public T2 ui;

            public DataChannel(IChangeTracker<T1> changeTracker, T2 ui)
            {
                this.changeTracker = changeTracker;
                this.ui = ui;

                changeTracker.OnChange += Invoke;
            }

            private void Invoke(T1 a) 
            {
                Blah?.Invoke(a, ui);
            }

            public void Dispose()
            {
                changeTracker.OnChange -= Invoke;
            }

            public event System.Action<T1, T2> Blah;
        }

        public void UpdateHoveringText(string hover) => hovering.text = string.IsNullOrEmpty(hover) ? settings.hoveringNothingText : hover;
        public void UpdateMoneyText(int money, int maxMoney) => this.money.text = $"({money} / {maxMoney})";
        public void UpdateTimerText(float time) => timer.text = time.ToString();
        public void UpdateItem(IItemArchitype item) => heldItem.sprite = item == null ? settings.holdingNothingSprite : item.Sprite;
        public void UpdateItemCount(int count) => itemCount.text = count.ToString();
        public void UpdateMaxCapacity(bool atCapacity) => itemCount.color = atCapacity ? Color.red : Color.black;

        public void Dispose()
        {
            moneyCentral.OnMoneyChanged -= UpdateMoneyText;
            interactor.OnHoverChange    -= UpdateHoveringText;
            tickClock.OnChange -= UpdateTimerText;
            hand.OnItemChange      -= UpdateItem;
            hand.OnCountChange     -= UpdateItemCount;
            hand.AtMaxCapacity     -= UpdateMaxCapacity;
        }
    }
}