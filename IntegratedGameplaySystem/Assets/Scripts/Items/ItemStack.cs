using System.Collections.Generic;
using UnityEngine;

namespace IntegratedGameplaySystem
{
    public struct ItemStack
    {
        public IItemArchitype item;
        public int count;

        public ItemStack(IItemArchitype item, int count)
        {
            this.item = item;
            this.count = count;
        }

        public bool AtCapacity() => item != null && count >= item.MaxStackSize;
        public void Clamp(int overrideCount = 0) 
        {
            if (overrideCount <= 0)
                overrideCount = item.MaxStackSize;

            count = Mathf.Clamp(count, 0, overrideCount);
        }
    }
}
