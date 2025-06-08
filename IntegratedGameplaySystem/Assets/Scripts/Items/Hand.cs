using System;
using UnityEngine;

namespace IntegratedGameplaySystem
{
    /// <summary>
    /// And then we can add something called a bag which has more items in it.
    /// </summary>
    public class Hand : IStartable, IDisposable, IItemHolder, IChangeTracker<ItemStack> //, IChangeTracker<int>, IChangeTracker<bool>
    {
        // EITHER Evnet onholdingChange or some typa interface so other scripts can get a reference
        // to this mainly the interactor and the display
        public event Action<ItemStack> OnChange;

        public Action<int> OnCountChange;
        public Action<bool> AtMaxCapacity;

        /// <summary>
        /// Make this use StackableItem
        /// </summary>
        private ItemStack heldItem;
        // and then the max count in in the thing.

        private int maxCount = 10;
        public bool isBoosted;

        public void Start()
        {
            EventManager<IItemArchitype>.AddListener(Occasion.PickupItem, SetOrAddItem);

            Clear();
            OnChange?.Invoke(heldItem);
        }

        private void SetOrAddItem(IItemArchitype newItem)
        {
            if (newItem != null && heldItem.item == newItem)
            {
                heldItem.count++;

                // or something.
                heldItem.Clamp(maxCount * (isBoosted ? 2 : 1));
            }
            else
            {
                heldItem.item = newItem;
                heldItem.count = newItem == null ? 0 : 1;
            }

            OnChange?.Invoke(heldItem);
        }

        public void Dispose()
        {
            EventManager<IItemArchitype>.RemoveListener(Occasion.PickupItem, SetOrAddItem);
        }

        public ItemStack[] GetItems()
        {
            return new ItemStack[] { heldItem };
        }

        public void Clear()
        {
            SetOrAddItem(null);
        }
    }
}