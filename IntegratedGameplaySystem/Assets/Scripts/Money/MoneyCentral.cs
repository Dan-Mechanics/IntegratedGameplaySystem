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

        public delegate int SellAll();
        public delegate bool HasSomethingToSell();
        private readonly ParticleSystem particle;
        private readonly MoneyCentralSettings settings;
        private int money;

        private readonly SellAll sellAll;
        private readonly HasSomethingToSell hasSomethingToSell;

        public string Name => hasSomethingToSell() ? "Sell crop" : string.Empty;

        public MoneyCentral(SellAll sellAll, HasSomethingToSell hasSomethingToSell)
        {
            this.sellAll = sellAll;
            this.hasSomethingToSell = hasSomethingToSell;

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

        /*public void Dispose()
        {
            EventManager<int>.RemoveListener(Occasion.EarnMoney, EarnMoney);
        }*/
        
        public void Interact()
        {
            if (!hasSomethingToSell())
                return;
            
            particle.Play();
            EarnMoney(sellAll());
        }
    }
}