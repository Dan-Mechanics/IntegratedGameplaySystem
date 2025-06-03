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
        private readonly MoneyCentral wallet;
        private readonly Interactor interactor;
        private readonly Clock tickClock;

        //public const string CANVAS_PREFAB_NAME = "canvas";
        private readonly Text hoveringText;
        private readonly Text moneyText;
        private readonly Text timerText;

        public Display(Interactor interactor, MoneyCentral wallet, Clock tickClock)
        {
            this.wallet = wallet;
            this.interactor = interactor;
            this.tickClock = tickClock;

            var settings = ServiceLocator<IAssetService>.Locate().GetAssetWithType<DisplaySettings>();
            var canvas = Utils.SpawnPrefab(settings.canvas).transform;

            // make something for this.
            hoveringText = Utils.AddTextToCanvas(canvas, settings.text, 0.5f * 30f * Vector2.down);
            moneyText = Utils.AddTextToCanvas(canvas, settings.text, 1.5f * 30f * Vector2.down);
            timerText = Utils.AddTextToCanvas(canvas, settings.text, 2f * 30f * Vector2.down);
        }

        public void Start()
        {
            wallet.OnMoneyChanged += UpdateMoneyText;
            interactor.OnHoverChange += UpdateHoveringText;
            tickClock.OnNewTime += UpdateTimerText;
            //EventManagerGeneric<int>.RaiseEvent(Occasion.EARN_MONEY, 10);
        }

        public void UpdateHoveringText(Transform hit) => hoveringText.text = hit ? hit.name : string.Empty;
        public void UpdateMoneyText(int money, int maxMoney) => moneyText.text = money > 0 ? $"({money} / {maxMoney})" : string.Empty;
        public void UpdateTimerText(float time) => timerText.text = time.ToString();

        public void Dispose()
        {
            wallet.OnMoneyChanged -= UpdateMoneyText;
            interactor.OnHoverChange -= UpdateHoveringText;
            tickClock.OnNewTime -= UpdateTimerText;
        }
    }
}