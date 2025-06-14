using System;

namespace IntegratedGameplaySystem
{
    /// <summary>
    /// Inventory with single slot.
    /// </summary>
    public class Hand : IStartable, IDisposable, IItemHolder, IChangeTracker<ItemStack>
    {
        public event Action<ItemStack> OnChange;
        public Action<int> onCountChange;
        public Action<bool> atMaxCapacity;

        private ItemStack heldItem;
        private IMaxStackSource maxStackSource;

        public Hand(HandSettings settings)
        {
            SetMaxStackSource(settings);
        }

        public void Start()
        {
            EventManager<IItemArchitype>.AddListener(Occasion.PickupItem, PickupItem);

            Clear();
            OnChange?.Invoke(heldItem);
        }

        public void SetMaxStackSource(IMaxStackSource maxStackSource) => this.maxStackSource = maxStackSource;

        private void PickupItem(IItemArchitype newItem)
        {
            if (newItem != null && heldItem.item == newItem)
            {
                heldItem.count++;
                heldItem.Clamp(maxStackSource.GetMaxStack());
            }
            else
            {
                heldItem.item = newItem;
                heldItem.count = newItem == null ? 0 : 1;
            }

            heldItem.isAtCapacity = heldItem.count >= maxStackSource.GetMaxStack();
            OnChange?.Invoke(heldItem);
        }

        public void Dispose()
        {
            EventManager<IItemArchitype>.RemoveListener(Occasion.PickupItem, PickupItem);
        }

        public ItemStack[] GetItems()
        {
            return new ItemStack[] { heldItem };
        }

        public void Clear()
        {
            PickupItem(null);
        }
    }
}