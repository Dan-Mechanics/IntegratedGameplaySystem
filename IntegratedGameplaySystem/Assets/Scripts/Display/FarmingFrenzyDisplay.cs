using UnityEngine;
using UnityEngine.UI;

namespace IntegratedGameplaySystem
{
    public class FarmingFrenzyDisplay : IStartable, IDisposable
    {
        private readonly Display display;
  
        private readonly DataChannel<string, Text> interactor;
        private readonly DataChannel<float, Text> score;
        private readonly DataChannel<ItemStack, ItemSlot> hand;
        private readonly DataChannel<IntWithMax, Image> moneyBar;
        private readonly DataChannel<IntWithMax, Text> moneyText;
        private readonly DataChannel<float, Text> sensitivity;

        public FarmingFrenzyDisplay(IChangeTracker<string> interactor, IChangeTracker<IntWithMax> money, IChangeTracker<float> score, 
            IChangeTracker<ItemStack> hand, IChangeTracker<float> sensitivity) 
        {
            display = new Display();

            this.interactor = new DataChannel<string, Text>(interactor, display.Disposables);
            this.score = new DataChannel<float, Text>(score, display.Disposables);
            this.hand = new DataChannel<ItemStack, ItemSlot>(hand, display.Disposables);
            this.sensitivity = new DataChannel<float, Text>(sensitivity, display.Disposables);
            moneyBar = new DataChannel<IntWithMax, Image>(money, display.Disposables);
            moneyText = new DataChannel<IntWithMax, Text>(money, display.Disposables);
        }

        public void Start()
        {
            InitializeUI(display.Settings, display.Canvas);

            interactor.OnChange += display.SettingsStrIntoText;
            moneyBar.OnChange += Display.RangeIntoFillImage;
            score.OnChange += Display.FloatIntoText;
            hand.OnChange += Display.ItemStackIntoSlot;
            moneyText.OnChange += Display.RangeIntoText;
            sensitivity.OnChange += Display.FloatIntoText;
        }

        /// <summary>
        /// Create all the game UI.
        /// </summary>
        private void InitializeUI(DisplaySettings settings, Transform canvas) 
        {
            interactor.ui = Display.AddToCanvas<Text>(canvas, settings.textPrefab);
            EasyRect rect = new EasyRect(interactor.ui.rectTransform);
            rect.SetSize(500f, rect.GetHeight());
            rect.SnapTo(Snap.Center);

            score.ui = Display.AddToCanvas<Text>(canvas, settings.textPrefab);
            rect.Set(score.ui.rectTransform);
            rect.SetSize(200, 30f);
            rect.SnapTo(Snap.Bottom, 200f * Vector2.right + (15f * Vector2.up));

            moneyBar.ui = Display.AddToCanvas<Image>(canvas, settings.imagePrefab);
            rect.Set(moneyBar.ui.rectTransform);
            moneyBar.ui.color = Color.black;
            rect.SetSize(200, 30f);
            rect.SnapTo(Snap.Bottom, 200f * Vector2.left + (15f * Vector2.up));
            
            moneyBar.ui = display.AddFillImage();
            rect.Set(moneyBar.ui.rectTransform);
            moneyBar.ui.color = Color.yellow;
            rect.SetSize(200f, 30f);
            rect.SnapTo(Snap.Bottom, 200f * Vector2.left + (15f * Vector2.up));

            moneyText.ui = Display.AddToCanvas<Text>(canvas, settings.textPrefab);
            rect.Set(moneyText.ui.rectTransform);
            moneyText.ui.color = Color.white;
            rect.SetSize(200, 30f);
            rect.SnapTo(Snap.Bottom, 200f * Vector2.left + (15f * Vector2.up));
            
            hand.ui.image = Display.AddToCanvas<Image>(canvas, settings.imagePrefab);
            rect.Set(hand.ui.image.rectTransform);
            rect.SnapTo(Snap.Bottom, Vector2.up * 15f);

            hand.ui.text = Display.AddToCanvas<Text>(canvas, settings.textPrefab);
            hand.ui.text.color = Color.white;
            hand.ui.text.fontSize = 40;
            rect.Set(hand.ui.text.rectTransform);
            rect.SnapTo(Snap.Bottom, Vector2.up * 15f);

            sensitivity.ui = Display.AddToCanvas<Text>(canvas, settings.textPrefab);
            rect.Set(sensitivity.ui.rectTransform);
            rect.SnapToTop();

            Image overlay = Display.AddToCanvas<Image>(canvas, settings.imagePrefab);
            overlay.sprite = settings.defaultSprite;
            overlay.color = Color.black;
            rect.Set(overlay.rectTransform);
            rect.SnapTo(Snap.Bottom, Vector2.up * 15f);
        }

        public void Dispose()
        {
            display.Dispose();

            interactor.OnChange -= display.SettingsStrIntoText;
            moneyBar.OnChange -= Display.RangeIntoFillImage;
            score.OnChange -= Display.FloatIntoText;
            hand.OnChange -= Display.ItemStackIntoSlot;
            moneyText.OnChange -= Display.RangeIntoText;
            sensitivity.OnChange -= Display.FloatIntoText;
        }
    }
}