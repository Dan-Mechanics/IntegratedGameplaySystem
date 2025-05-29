using System;
using UnityEngine;

namespace IntegratedGameplaySystem
{
    /// <summary>
    /// This class should respond to the events yo.
    /// </summary>
    public class Wallet : IDisposable
    {
        public Action<int, int> OnMoneyChanged;
        
        public int moneyToWin;
        private int money;

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public void EarnMoney(int incoming) 
        {
            if (incoming <= 0)
                return;

            money += incoming;

            money = Mathf.Clamp(money, 0, moneyToWin);
            if (money >= moneyToWin)
                EventManager.RaiseEvent(Occasion.GAME_OVER);

            OnMoneyChanged?.Invoke(money, moneyToWin);
        }
    }
}