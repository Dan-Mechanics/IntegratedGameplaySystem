using System;
using UnityEngine;

namespace IntegratedGameplaySystem
{
    public class MoneyCentral :  IStartable, IInteractable, IHoverable, IDisposable, IChangeTracker<IntWithMax>
    {
        public event Action<IntWithMax> OnChange;

        private readonly ParticleSystem particle;
        private readonly MoneyCentralSettings settings;
        private readonly IItemHolder itemHolder;

        private IntWithMax money;

        public string GetHoverTitle() 
        {
            return CanInteract() ? "Sell crop" : "No crop to sell";
        }

        public MoneyCentral(IItemHolder itemHolder)
        {
            this.itemHolder = itemHolder;
            settings = ServiceLocator<IAssetService>.Locate().GetAssetByType<MoneyCentralSettings>();

            GameObject go = Utils.SpawnPrefab(settings.prefab);
            particle = go.transform.GetComponentInChildren<ParticleSystem>();
            money.max = settings.moneyToWin;

            ServiceLocator<IWorldService>.Locate().Add(go.transform, this);
        }

        public void Start()
        {
            OnChange?.Invoke(money);
            EventManager<int>.AddListener(Occasion.EarnMoney, EarnMoney);
            EventManager<int>.AddListener(Occasion.LoseMoney, LoseMoney);
        }

        public bool CanAfford(int cost) 
        {
            return money.value >= cost;
        }

        private void EarnMoney(int amount)
        {
            if (amount <= 0)
                return;

            money.value += amount;
            money.Clamp();
            OnChange?.Invoke(money);

            if (money.value >= money.max)
                EventManager.RaiseEvent(Occasion.GameOver);
        }

        private void LoseMoney(int amount)
        {
            if (amount <= 0)
                return;

            money.value -= amount;
            money.Clamp();
            OnChange?.Invoke(money);
        }

        public void Interact()
        {
            if (!CanInteract())
                return;
            
            particle.Play();
            EventManager<int>.RaiseEvent(Occasion.EarnMoney, GetEarnings());
        }

        public bool CanInteract() 
        {
            int count = 0;

            ItemStack[] stacks = itemHolder.GetItems();
            ItemStack stack;

            for (int i = 0; i < stacks.Length; i++)
            {
                stack = stacks[i];
                if (stack.item != null)
                    count += stack.count;
            }

            return count > 0;
        }

        public int GetEarnings() 
        {
            int earnings = 0;

            ItemStack[] stacks = itemHolder.GetItems();
            ItemStack stack;

            for (int i = 0; i < stacks.Length; i++)
            {
                stack = stacks[i];
                if (stack.item == null)
                    continue;

                earnings += stack.count * stack.item.MonetaryValue;
            }

            itemHolder.Clear();

            return earnings;
        }

        /// <summary>
        /// I think this works chat.
        /// Hopefully it's clear how this would work.
        /// </summary>
        public void Dispose()
        {
            EventManager<int>.RemoveListener(Occasion.EarnMoney, EarnMoney);
            EventManager<int>.RemoveListener(Occasion.LoseMoney, LoseMoney);
        }
    }
}