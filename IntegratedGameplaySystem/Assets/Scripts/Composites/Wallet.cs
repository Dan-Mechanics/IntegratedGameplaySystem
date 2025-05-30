using System;
using UnityEngine;

namespace IntegratedGameplaySystem
{
    /// <summary>
    /// This class should respond to the events yo.
    /// </summary>
    public class Wallet : IDisposable
    {
        public event Action<int, int> OnMoneyChanged;
        
        public int moneyToWin;
        public int moneyPerPlantGained;

        private int money;

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public void Collect() => EarnMoney(moneyPerPlantGained);

        public void EarnMoney(int incoming) 
        {
            if (incoming <= 0)
                return;

            money += incoming;

            money = Mathf.Clamp(money, 0, moneyToWin);
            OnMoneyChanged?.Invoke(money, moneyToWin);

            if (money >= moneyToWin)
                EventManager.RaiseEvent(Occasion.GAME_OVER);
        }
    }
}