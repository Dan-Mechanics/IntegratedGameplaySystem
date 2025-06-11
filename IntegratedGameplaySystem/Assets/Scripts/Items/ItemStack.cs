using UnityEngine;

namespace IntegratedGameplaySystem
{
    public struct ItemStack
    {
        public IItemArchitype item;
        public int count;
        public bool isAtCapacity;

        public ItemStack(IItemArchitype item, int count, bool isAtCapacity)
        {
            this.item = item;
            this.count = count;
            this.isAtCapacity = isAtCapacity;
        }

        public void Clamp(int max) => count = Mathf.Clamp(count, 0, max);
    }
}
