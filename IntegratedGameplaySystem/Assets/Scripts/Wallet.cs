using System;
using UnityEngine;

namespace IntegratedGameplaySystem
{
    public class Wallet 
    {
        public Action<int, int> OnMoneyChanged;
        
        public readonly int moneyToWin;
        private int money;

        public Wallet(int moneyToWin)
        {
            this.moneyToWin = moneyToWin;
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