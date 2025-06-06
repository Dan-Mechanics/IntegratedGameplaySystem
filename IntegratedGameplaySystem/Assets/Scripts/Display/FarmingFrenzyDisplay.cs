using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace IntegratedGameplaySystem
{
    public class FarmingFrenzyDisplay : IStartable, IDisposable
    {
        private readonly BaseDisplay baseDisplay = new BaseDisplay();
        
        //private readonly List<IDisposable> disposables = new();

        private readonly DataChannel<string, Text> interactor;
        private readonly DataChannel<float, Text> score;
        private readonly DataChannel<ItemStack, Slot> hand;
        private readonly DataChannel<Range, Image> moneyBar;
        private readonly DataChannel<Range, Text> moneyText;

       // public DisplaySettings settings;
       // public Transform canvas;

        public FarmingFrenzyDisplay(IChangeTracker<string> interactor, IChangeTracker<Range> money, IChangeTracker<float> score, 
            IChangeTracker<ItemStack> hand) 
        {
            this.interactor = new DataChannel<string, Text>(interactor, baseDisplay.Disposables);
            this.score = new DataChannel<float, Text>(score, baseDisplay.Disposables);
            this.hand = new DataChannel<ItemStack, Slot>(hand, baseDisplay.Disposables);
            moneyBar = new DataChannel<Range, Image>(money, baseDisplay.Disposables);
            moneyText = new DataChannel<Range, Text>(money, baseDisplay.Disposables);

            InitializeUI(baseDisplay.Settings, baseDisplay.Canvas);
        }
        
        /// <summary>
        /// Amazing code here.
        /// </summary>
        private void InitializeUI(DisplaySettings settings, Transform canvas) 
        {
            interactor.ui = BaseDisplay.AddToCanvas<Text>(canvas, settings.text);
            EasyRect rect = new EasyRect(interactor.ui.rectTransform);
            rect.SnapTo(Snap.Center, 1f * rect.GetHeight() * Vector2.down);

            score.ui = BaseDisplay.AddToCanvas<Text>(canvas, settings.text);
            rect.Set(score.ui.rectTransform);
            rect.SetSize(200, 30f);
            rect.SnapTo(Snap.Bottom, 200f * Vector2.right + (15f * Vector2.up));

            moneyBar.ui = BaseDisplay.AddToCanvas<Image>(canvas, settings.image);
            rect.Set(moneyBar.ui.rectTransform);
            moneyBar.ui.color = Color.black;
            rect.SetSize(200, 30f);
            rect.SnapTo(Snap.Bottom, 200f * Vector2.left + (15f * Vector2.up));
            
            moneyBar.ui = BaseDisplay.AddFillImage(canvas, settings.image, settings.pixel);
            rect.Set(moneyBar.ui.rectTransform);
            moneyBar.ui.color = Color.yellow;
            rect.SetSize(200f, 30f);
            rect.SnapTo(Snap.Bottom, 200f * Vector2.left + (15f * Vector2.up));

            moneyText.ui = BaseDisplay.AddToCanvas<Text>(canvas, settings.text);
            rect.Set(moneyText.ui.rectTransform);
            moneyText.ui.color = Color.white;
            rect.SetSize(200, 30f);
            rect.SnapTo(Snap.Bottom, 200f * Vector2.left + (15f * Vector2.up));
            
            hand.ui.image = BaseDisplay.AddToCanvas<Image>(canvas, settings.image);
            rect.Set(hand.ui.image.rectTransform);
            rect.SnapTo(Snap.Bottom, Vector2.up * 15f);

            hand.ui.text = BaseDisplay.AddToCanvas<Text>(canvas, settings.text);
            hand.ui.text.color = Color.white;
            hand.ui.text.fontSize = 40;
            rect.Set(hand.ui.text.rectTransform);
            rect.SnapTo(Snap.Bottom, Vector2.up * 15f);

            Image overlay = BaseDisplay.AddToCanvas<Image>(canvas, settings.image);
            overlay.sprite = settings.defaultSprite;
            rect.Set(overlay.rectTransform);
            rect.SnapTo(Snap.Bottom, Vector2.up * 15f);
        }

        public void Start()
        {
            moneyBar.OnChange += BaseDisplay.RangeIntoFillImage;
            interactor.OnChange += BaseDisplay.StringIntoText;
            score.OnChange += BaseDisplay.FloatIntoText;
            hand.OnChange += BaseDisplay.ItemStackIntoSlot;
            moneyText.OnChange += BaseDisplay.RangeIntoText;
        }

        public void Dispose()
        {
            baseDisplay.Dispose();

            moneyBar.OnChange -= BaseDisplay.RangeIntoFillImage;
            interactor.OnChange -= BaseDisplay.StringIntoText;
            score.OnChange -= BaseDisplay.FloatIntoText;
            hand.OnChange -= BaseDisplay.ItemStackIntoSlot;
            moneyText.OnChange -= BaseDisplay.RangeIntoText;
        }
    }
}