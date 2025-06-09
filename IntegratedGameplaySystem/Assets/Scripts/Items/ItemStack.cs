using System.Collections.Generic;
using UnityEngine;

namespace IntegratedGameplaySystem
{
    public struct ItemStack
    {
        public IItemArchitype item;
        public int count;
        public bool isAtCapacity;

        public ItemStack(IItemArchitype item, int count, bool atCapacity)
        {
            this.item = item;
            this.count = count;
            this.isAtCapacity = atCapacity;
        }


        //public bool AtCapacity(int max) => item != null && count >= max;
        public void Clamp(int max) => count = Mathf.Clamp(count, 0, max);
    }
}
