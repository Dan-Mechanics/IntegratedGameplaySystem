using System;
using System.Collections.Generic;
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
        private readonly List<IDisposable> disposables = new();

        private readonly DataChannel<string, Text> interactor;
        private readonly DataChannel<float, Text> score;
        private readonly DataChannel<ItemStack, Slot> hand;
        private readonly DataChannel<Range, Text> money;
        
        private DisplaySettings settings;
        
        public Display(IChangeTracker<string> interactor, IChangeTracker<Range> money, IChangeTracker<float> score, 
            IChangeTracker<ItemStack> hand)
        {
            this.interactor = new DataChannel<string, Text>(interactor, disposables);
            this.money = new DataChannel<Range, Text>(money, disposables);
            this.score = new DataChannel<float, Text>(score, disposables);
            this.hand = new DataChannel<ItemStack, Slot>(hand, disposables);
            //this.atCapacity = new DataChannel<bool, Text>(atCapacity, disposables);

            Setup();
        }

        private void Setup() 
        {
            settings = ServiceLocator<IAssetService>.Locate().GetAssetByType<DisplaySettings>();
            Transform canvas = Utils.SpawnPrefab(settings.canvas).transform;


            // Amazing code here:
            interactor.ui = Utils.AddToCanvas<Text>(canvas, settings.text);
            EasyUI temp = new EasyUI(interactor.ui.rectTransform);
            temp.SnapTo(Snap.Center, 1f * temp.GetHeight() * Vector2.down);

            score.ui = Utils.AddToCanvas<Text>(canvas, settings.text);
            temp.Set(score.ui.rectTransform);
            temp.SnapTo(Snap.Center, 2f * temp.GetHeight() * Vector2.down);

            money.ui = Utils.AddToCanvas<Text>(canvas, settings.text);
            temp.Set(money.ui.rectTransform);
            temp.SnapTo(Snap.Center, 3f * temp.GetHeight() * Vector2.down);

            hand.ui.image = Utils.AddToCanvas<Image>(canvas, settings.image);
            temp.Set(hand.ui.image.rectTransform);
            temp.SnapTo(Snap.Bottom, Vector2.up * 15f);

            Vector2 itemCountOffset = (15f + temp.GetHeight() / 2f) * (Vector2.up + Vector2.left);

            hand.ui.text = Utils.AddToCanvas<Text>(canvas, settings.text);
            hand.ui.text.color = Color.black;
            temp.Set(hand.ui.text.rectTransform);
            temp.SnapTo(Snap.Bottom, itemCountOffset);

            Image overlay = Utils.AddToCanvas<Image>(canvas, settings.image);
            overlay.sprite = settings.defaultSprite;
            temp.Set(overlay.rectTransform);
            temp.SnapTo(Snap.Bottom, Vector2.up * 15f);
        }

        public void Start()
        {
            money.OnChange += RangeIntoText;
            interactor.OnChange += StringIntoText;
            score.OnChange += FloatIntoText;
            hand.OnChange += ItemStackIntoSlot;
        }

        public void StringIntoText(string str, Text text) => text.text = string.IsNullOrEmpty(str) ? settings.defaultText : str;
        public void RangeIntoText(Range range, Text text) => text.text = $"({range.value} / {range.max})";
        public void FloatIntoText(float value, Text text) => text.text = value.ToString();
        public void IntIntoText(int value, Text text) => text.text = value.ToString();
        public void SpriteIntoImage(Sprite sprite, Image image) => image.sprite = sprite == null ? settings.defaultSprite : sprite;

        /// <summary>
        /// Or something ...
        /// </summary>
        public void BoolIntoRedText(bool isRed, Text text) => text.color = isRed ? Color.red : Color.black;
        
        public void ItemStackIntoSlot(ItemStack stack, Slot slot) 
        {
            SpriteIntoImage(stack.item?.Sprite, slot.image);
            IntIntoText(stack.count, slot.text);
            BoolIntoRedText(stack.AtCapacity(), slot.text);
        }

        public void Dispose()
        {
            disposables.ForEach(x => x.Dispose());

            money.OnChange -= RangeIntoText;
            interactor.OnChange -= StringIntoText;
            score.OnChange -= FloatIntoText;
            hand.OnChange -= ItemStackIntoSlot;
        }

        /// <summary>
        /// T1 --> T2
        /// </summary>
        public class DataChannel<T1, T2> : IDisposable
        {
            private readonly IChangeTracker<T1> changeTracker;
            public T2 ui;

            public DataChannel(IChangeTracker<T1> changeTracker, List<IDisposable> disposables)
            {
                this.changeTracker = changeTracker;
                changeTracker.OnChange += Combine;

                disposables.Add(this);
            }

            private void Combine(T1 a) => OnChange?.Invoke(a, ui);

            public void Dispose() => changeTracker.OnChange -= Combine;

            public event Action<T1, T2> OnChange;
        }
    }
}