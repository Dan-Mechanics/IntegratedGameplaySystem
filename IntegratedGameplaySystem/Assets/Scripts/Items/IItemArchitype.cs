using System.Collections.Generic;
using UnityEngine;

namespace IntegratedGameplaySystem
{
    public interface IItemArchitype
    {
        public Sprite Sprite { get; }
        // public int MaxStackSize { get; }
        public int MonetaryValue { get; }
    }
}
