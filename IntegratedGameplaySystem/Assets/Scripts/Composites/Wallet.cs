using System;
using System.Collections.Generic;
using UnityEngine;

namespace IntegratedGameplaySystem
{
    /// <summary>
    /// This class should respond to the events yo.
    /// </summary>
    public class Wallet : IDisposable, IStartable
    {
        public event Action<int, int> OnMoneyChanged;
        
        public int moneyToWin;
        public int moneyPerPlantGained;

        private int money;
        private readonly List<Plant> plants;

        public Wallet(int moneyToWin, int moneyPerPlantGained, List<Plant> plants)
        {
            this.plants = plants;
            this.moneyToWin = moneyToWin;
            this.moneyPerPlantGained = moneyPerPlantGained;

            plants.ForEach(x => x.OnCollect += Collect);
        }

        public void Dispose()
        {
            plants.ForEach(x => x.OnCollect -= Collect);
            plants.Clear();
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

        public void Start()
        {
            OnMoneyChanged += Log;
        }

        private void Log(int arg1, int arg2)
        {
            Debug.Log(arg1);
        }
    }
}