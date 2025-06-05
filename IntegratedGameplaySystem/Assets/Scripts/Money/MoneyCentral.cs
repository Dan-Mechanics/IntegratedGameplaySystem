using System;
using UnityEngine;

namespace IntegratedGameplaySystem
{
    /// <summary>
    /// This class should respond to the events yo.
    /// 
    /// Barter interface
    /// </summary>
    public class MoneyCentral :  IStartable, IInteractable, IHoverable, IDisposable, IChangeTracker<Range>
    {
        public event Action<Range> OnChange;

        private readonly ParticleSystem particle;
        private readonly MoneyCentralSettings settings;
        private readonly IItemHolder itemHolder;
        //private int money;

        private Range money;

        // public Func<bool> CanInteract;
        // public Func<int> GetEarnings;

        // use Func ??? i dont fucking know how it works.

        public string HoverTitle => CanInteract() ? "Sell crop" : "No crop to sell";

        /// <summary>
        /// Do we need something to deallocate sellall ??
        /// Dont use delegates i dont understand them bruuhhhhh
        /// Like it works but i dont know what happens to the memory AT ALL
        /// </summary>
        public MoneyCentral(IItemHolder itemHolder)
        {
            this.itemHolder = itemHolder;
            settings = ServiceLocator<IAssetService>.Locate().GetAssetWithType<MoneyCentralSettings>();

            GameObject go = Utils.SpawnPrefab(settings.prefab);
            particle = go.transform.GetComponentInChildren<ParticleSystem>();
            money.max = settings.moneyToWin;

            ServiceLocator<IWorldService>.Locate().Add(go, this);
        }

        public void Start()
        {
            OnChange?.Invoke(money);
            EventManager<int>.AddListener(Occasion.EarnMoney, EarnMoney);
        }

        private void EarnMoney(int incoming)
        {
            if (incoming <= 0)
                return;

            money.value += incoming;
            money.Clamp();
            OnChange?.Invoke(money);

            if (money.value >= money.max)
                EventManager.RaiseEvent(Occasion.GameOver);
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
        }
    }
}