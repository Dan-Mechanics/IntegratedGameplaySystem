using System;
using UnityEngine;

namespace IntegratedGameplaySystem
{
    /// <summary>
    /// This class should respond to the events yo.
    /// </summary>
    public class Wallet :  IStartable, IDisposable
    {
        public event Action<int, int> OnMoneyChanged;

        private readonly WalletSettings settings;
        private int money;

        public Wallet()
        {
            settings = ServiceLocator<IAssetService>.Locate().GetByType<WalletSettings>();
        }

        public void Start()
        {
            OnMoneyChanged?.Invoke(money, settings.moneyToWin);
            EventManagerGeneric<int>.AddListener(Occasion.EARN_MONEY, EarnMoney);
        }

        public void EarnMoney(int incoming)
        {
            if (incoming <= 0)
                return;

            money += incoming;

            money = Mathf.Clamp(money, 0, settings.moneyToWin);
            OnMoneyChanged?.Invoke(money, settings.moneyToWin);

            if (money >= settings.moneyToWin)
                EventManager.RaiseEvent(Occasion.GAME_OVER);
        }

        public void Dispose()
        {
            EventManagerGeneric<int>.RemoveListener(Occasion.EARN_MONEY, EarnMoney);
        }
    }
}