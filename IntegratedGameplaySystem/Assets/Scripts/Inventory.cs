using System;
using UnityEngine;

namespace IntegratedGameplaySystem
{
    public class Inventory : IStartable, IDisposable
    {
        // EITHER Evnet onholdingChange or some typa interface so other scripts can get a reference
        // to this mainly the interactor and the display
        public Action<ISellableItem> OnItemChange;
        public Action<int> OnCountChange;
        public Action<bool> AtMaxCapacity;

        private ISellableItem currentItem;
        private int count;
        // and then the max count in in the thing.

        public void Start()
        {
            EventManager<ISellableItem>.AddListener(Occasion.SetOrAddItem, SetOrAddItem);
            EventManager<ISellableItem>.RaiseEvent(Occasion.SetOrAddItem, null);

            OnItemChange?.Invoke(currentItem);
            OnCountChange?.Invoke(count);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>The amount of money worth all the inventory.</returns>
        public int SellAll() 
        {
            int money = 0;

            if (currentItem != null)
                money = currentItem.MaxCount * count;

            EventManager<ISellableItem>.RaiseEvent(Occasion.SetOrAddItem, null);

            return money;
        }

        public bool HasSomethingToSell()
        {
            return currentItem != null && count > 0;
        }

        private void SetOrAddItem(ISellableItem newItem)
        {
            if (newItem != null && currentItem == newItem)
            {
                count++;

                // GIVE WARNING HERE !!
                if (count > currentItem.MaxCount)
                    count = currentItem.MaxCount;
            }
            else
            {
                currentItem = newItem;
                count = currentItem == null ? 0 : 1;
            }

            AtMaxCapacity?.Invoke(currentItem != null && count >= currentItem.MaxCount);
            OnItemChange?.Invoke(currentItem);
            OnCountChange?.Invoke(count);
        }

        public void Dispose()
        {
            EventManager<ISellableItem>.RemoveListener(Occasion.SetOrAddItem, SetOrAddItem);
        }
    }
}