using System;
using UnityEngine;

namespace IntegratedGameplaySystem
{
    /// <summary>
    /// This class should respond to the events yo.
    /// </summary>
    public class MoneyCentral :  IStartable, IInteractable, IHoverable
    {
        public event Action<int, int> OnMoneyChanged;

        private readonly ParticleSystem particle;
        private readonly MoneyCentralSettings settings;
        private int money;

        public delegate int SellAll();
        public delegate bool HasSomethingToSell();
        private readonly SellAll sellAll;
        private readonly HasSomethingToSell hasSomethingToSell;

        // use Func ??? i dont fucking know how it works.

        public string HoverTitle => hasSomethingToSell() ? "Sell crop" : "No crop to sell";

        /// <summary>
        /// Do we need something to deallocate sellall ??
        /// Dont use delegates i dont understand them bruuhhhhh
        /// Like it works but i dont know what happens to the memory AT ALL
        /// </summary>
        public MoneyCentral(SellAll sellAll, HasSomethingToSell hasSomethingToSell)
        {
            this.sellAll = sellAll;
            this.hasSomethingToSell = hasSomethingToSell;

            // ???
            /*this.sellAll += sellAll;
            this.sellAll -= sellAll;*/

            settings = ServiceLocator<IAssetService>.Locate().GetAssetWithType<MoneyCentralSettings>();

            GameObject go = Utils.SpawnPrefab(settings.prefab);
            particle = go.transform.GetComponentInChildren<ParticleSystem>();

            ServiceLocator<IWorldService>.Locate().Add(go, this);
        }

        public void Start()
        {
            OnMoneyChanged?.Invoke(money, settings.moneyToWin);
            //EventManager<int>.AddListener(Occasion.EarnMoney, EarnMoney);
        }

        public void EarnMoney(int incoming)
        {
            if (incoming <= 0)
                return;

            money += incoming;

            money = Mathf.Clamp(money, 0, settings.moneyToWin);
            OnMoneyChanged?.Invoke(money, settings.moneyToWin);

            if (money >= settings.moneyToWin)
                EventManager.RaiseEvent(Occasion.GameOver);
        }

        public void Interact()
        {
            if (!hasSomethingToSell())
                return;
            
            particle.Play();
            EarnMoney(sellAll());
        }
    }
}