using System;
using UnityEngine;

namespace IntegratedGameplaySystem
{
    [Serializable]
    public class Wallet 
    {
        public Action<int, int> OnMoneyChanged;
        
        public int moneyToWin;
        private int money;

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