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
        private ItemStack heldItem;
        // and then the max count in in the thing.
        
        public void Start()
        {
            EventManager<IItemArchitype>.AddListener(Occasion.SetOrAddItem, SetOrAddItem);
            Clear();

            OnItemChange?.Invoke(heldItem.item);
            OnCountChange?.Invoke(heldItem.count);
        }

        private void SetOrAddItem(IItemArchitype newItem)
        {
            if (newItem != null && heldItem.item == newItem)
            {
                heldItem.count++;
                heldItem.Clamp();
            }
            else
            {
                heldItem.item = newItem;
                heldItem.count = newItem == null ? 0 : 1;
            }

            AtMaxCapacity?.Invoke(heldItem.AtCapacity());
            OnItemChange?.Invoke(heldItem.item);
            OnCountChange?.Invoke(heldItem.count);
        }

        public void Dispose()
        {
            EventManager<IItemArchitype>.RemoveListener(Occasion.SetOrAddItem, SetOrAddItem);
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