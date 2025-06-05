using IntegratedGameplaySystem;
using System.Collections;
using UnityEngine;

namespace IntegratedGameplaySystem
{
    /// <summary>
    /// Je wilt misschien dat ze samen yappen met een interface, dat is pretty cool.
    /// ik denk wel handig dat de shit die subscribed de shit ook destroyed thats good
    /// Dus ze yappen via hun interface samen.
    /// </summary>
    public interface IItemHolder
    {
        ItemStack[] GetItems();
        void Clear();
    }
}