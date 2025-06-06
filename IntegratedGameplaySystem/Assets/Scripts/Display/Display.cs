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
        private readonly DataChannel<Range, Image> moneyBar;
        private readonly DataChannel<Range, Text> moneyText;

        public DisplaySettings settings;
        public Transform canvas;

        public Display(IChangeTracker<string> interactor, IChangeTracker<Range> money, IChangeTracker<float> score, 
            IChangeTracker<ItemStack> hand) 
        {
            this.interactor = new DataChannel<string, Text>(interactor, disposables);
            this.score = new DataChannel<float, Text>(score, disposables);
            this.hand = new DataChannel<ItemStack, Slot>(hand, disposables);
            moneyBar = new DataChannel<Range, Image>(money, disposables);
            moneyText = new DataChannel<Range, Text>(money, disposables);

            Setup();
        }
        
        /// <summary>
        /// Amazing code here.
        /// </summary>
        private void Setup() 
        {
            settings = ServiceLocator<IAssetService>.Locate().GetAssetByType<DisplaySettings>();
            Transform canvas = Utils.SpawnPrefab(settings.canvas).transform;

            interactor.ui = AddToCanvas<Text>(canvas, settings.text);
            EasyRect rect = new EasyRect(interactor.ui.rectTransform);
            rect.SnapTo(Snap.Center, 1f * rect.GetHeight() * Vector2.down);

            score.ui = AddToCanvas<Text>(canvas, settings.text);
            rect.Set(score.ui.rectTransform);
            rect.SetSize(200, 30f);
            rect.SnapTo(Snap.Bottom, 200f * Vector2.right + (15f * Vector2.up));

            moneyBar.ui = AddToCanvas<Image>(canvas, settings.image);
            rect.Set(moneyBar.ui.rectTransform);
            moneyBar.ui.color = Color.black;
            rect.SetSize(200, 30f);
            rect.SnapTo(Snap.Bottom, 200f * Vector2.left + (15f * Vector2.up));
            
            moneyBar.ui = AddFillImage(canvas, settings.image, settings.pixel);
            rect.Set(moneyBar.ui.rectTransform);
            moneyBar.ui.color = Color.yellow;
            rect.SetSize(200f, 30f);
            rect.SnapTo(Snap.Bottom, 200f * Vector2.left + (15f * Vector2.up));

            moneyText.ui = AddToCanvas<Text>(canvas, settings.text);
            rect.Set(moneyText.ui.rectTransform);
            moneyText.ui.color = Color.white;
            rect.SetSize(200, 30f);
            rect.SnapTo(Snap.Bottom, 200f * Vector2.left + (15f * Vector2.up));
            
            hand.ui.image = AddToCanvas<Image>(canvas, settings.image);
            rect.Set(hand.ui.image.rectTransform);
            rect.SnapTo(Snap.Bottom, Vector2.up * 15f);

            //Vector2 itemCountOffset = (15f + rect.GetHeight() / 2f) * (Vector2.up + Vector2.left);

            hand.ui.text = AddToCanvas<Text>(canvas, settings.text);
            hand.ui.text.color = Color.white;
            hand.ui.text.fontSize = 40;
            rect.Set(hand.ui.text.rectTransform);
            rect.SnapTo(Snap.Bottom, Vector2.up * 15f);

            Image overlay = AddToCanvas<Image>(canvas, settings.image);
            overlay.sprite = settings.defaultSprite;
            rect.Set(overlay.rectTransform);
            rect.SnapTo(Snap.Bottom, Vector2.up * 15f);
        }

        /// <summary>
        /// We might be able to make this class a little more modualir if we break
        /// this sub / unsub rule. That's really annoying me atm.
        /// </summary>
        public void Start()
        {
            moneyBar.OnChange += RangeIntoFillImage;
            interactor.OnChange += StringIntoText;
            score.OnChange += FloatIntoText;
            hand.OnChange += ItemStackIntoSlot;
            moneyText.OnChange += RangeIntoText;
        }

        public void StringIntoText(string str, Text text) => text.text = string.IsNullOrEmpty(str) ? settings.defaultText : str;
        public void RangeIntoText(Range range, Text text) => text.text = $"({range.value} / {range.max})";
        public void FloatIntoText(float value, Text text) => text.text = value.ToString();
        public void RangeIntoFillImage(Range range, Image fillImg) => fillImg.fillAmount = (float)range.value / range.max;
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

            moneyBar.OnChange -= RangeIntoFillImage;
            interactor.OnChange -= StringIntoText;
            score.OnChange -= FloatIntoText;
            hand.OnChange -= ItemStackIntoSlot;
            moneyText.OnChange -= RangeIntoText;
        }

        public static T AddToCanvas<T>(Transform canvas, GameObject prefab)
        {
            Transform transform = Utils.SpawnPrefab(prefab).transform;
            transform.SetParent(canvas);
            transform.localPosition = Vector3.zero;
            return transform.GetComponent<T>();
        }

        public static Image AddFillImage(Transform canvas, GameObject prefab, Sprite sprite)
        {
            Image image = AddToCanvas<Image>(canvas, prefab);
            image.sprite = sprite;

            image.type = Image.Type.Filled;
            image.fillAmount = 0f;
            image.fillOrigin = 0;
            image.fillMethod = Image.FillMethod.Horizontal;

            return image;
        }
    }
}