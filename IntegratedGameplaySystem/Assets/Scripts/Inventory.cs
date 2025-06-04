using System;
using UnityEngine;

namespace IntegratedGameplaySystem
{
    /// <summary>
    /// Rename to inventory.
    /// </summary>
    public class Inventory : IStartable, IDisposable
    {
        // EITHER Evnet onholdingChange or some typa interface so other scripts can get a reference
        // to this mainly the interactor and the display
        public Action<IItem> OnHold;
        public Action<int> OnCountChange;
        public Action<LayerMask?> OnNewSelectionMask;

        private IItem holding;
        private int count;
        // and then the max count in in the thing.

        public void Start()
        {
            EventManager<IItem>.AddListener(Occasion.EquipItem, Equip);

            OnHold?.Invoke(holding);
            OnCountChange?.Invoke(count);
        }

        public int SellAll() 
        {
            int money = 0;

            if (holding != null)
                money = holding.MaxCount * count;

            holding = null;
            count = 0;

            OnNewSelectionMask?.Invoke(null);
            OnHold?.Invoke(holding);
            OnCountChange?.Invoke(count);

            return money;
        }

        public bool HasSomethingToSell()
        {
            return holding != null && count > 0;
        }

        private void Equip(IItem item)
        {
            if (holding == item)
            {
                count++;

                if (count > holding.MaxCount)
                    count = holding.MaxCount;
            }
            else
            {
                OnNewSelectionMask?.Invoke(item.Mask);
                holding = item;
                count = 1;
            }

            OnHold?.Invoke(holding);
            OnCountChange?.Invoke(count);
        }

        public void Dispose()
        {
            EventManager<IItem>.RemoveListener(Occasion.EquipItem, Equip);
        }
    }
}