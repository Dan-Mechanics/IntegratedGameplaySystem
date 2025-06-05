using System;
using UnityEngine;

namespace IntegratedGameplaySystem
{
    /// <summary>
    /// And then we can add something called a bag which has more items in it.
    /// </summary>
    public class Hand : IStartable, IDisposable, IItemHolder
    {
        // EITHER Evnet onholdingChange or some typa interface so other scripts can get a reference
        // to this mainly the interactor and the display
        public Action<IItemArchitype> OnItemChange;
        public Action<int> OnCountChange;
        public Action<bool> AtMaxCapacity;

        /// <summary>
        /// Make this use StackableItem
        /// </summary>
        private IItemArchitype heldItem;
        private int count;
        // and then the max count in in the thing.

        public void Start()
        {
            EventManager<IItemArchitype>.AddListener(Occasion.SetOrAddItem, SetOrAddItem);
            Clear();

            OnItemChange?.Invoke(heldItem);
            OnCountChange?.Invoke(count);
        }

        private void SetOrAddItem(IItemArchitype newItem)
        {
            if (newItem != null && heldItem == newItem)
            {
                count++;

                // GIVE WARNING HERE !!
                if (count > heldItem.StackSize)
                    count = heldItem.StackSize;
            }
            else
            {
                heldItem = newItem;
                count = heldItem == null ? 0 : 1;
            }

            AtMaxCapacity?.Invoke(heldItem != null && count >= heldItem.StackSize);
            OnItemChange?.Invoke(heldItem);
            OnCountChange?.Invoke(count);
        }

        public void Dispose()
        {
            EventManager<IItemArchitype>.RemoveListener(Occasion.SetOrAddItem, SetOrAddItem);
        }

        public StackingItemInstance[] GetItems()
        {
            return new StackingItemInstance[] { new StackingItemInstance(heldItem, count) };
        }

        public void Clear()
        {
            SetOrAddItem(null);
        }
    }
}