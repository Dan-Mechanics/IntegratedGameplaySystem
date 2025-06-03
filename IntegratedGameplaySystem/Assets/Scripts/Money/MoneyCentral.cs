using System;
using UnityEngine;

namespace IntegratedGameplaySystem
{
    /// <summary>
    /// This class should respond to the events yo.
    /// </summary>
    public class MoneyCentral :  IStartable, IDisposable, IInteractable
    {
        public event Action<int, int> OnMoneyChanged;

        private readonly MoneyCentralSettings settings;
        private int money;

        private readonly ParticleSystem particle;

        public MoneyCentral()
        {
            settings = ServiceLocator<IAssetService>.Locate().GetAssetWithType<MoneyCentralSettings>();

            GameObject go = Utils.SpawnPrefab(settings.prefab);
            particle = go.transform.GetComponentInChildren<ParticleSystem>();

            ServiceLocator<IWorldService>.Locate().Add(go, this);
        }

        public void Start()
        {
            OnMoneyChanged?.Invoke(money, settings.moneyToWin);
            EventManager<int>.AddListener(Occasion.EARN_MONEY, EarnMoney);
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
            EventManager<int>.RemoveListener(Occasion.EARN_MONEY, EarnMoney);
        }
            
        public void Interact()
        {
            particle.Play();
        }
    }
}