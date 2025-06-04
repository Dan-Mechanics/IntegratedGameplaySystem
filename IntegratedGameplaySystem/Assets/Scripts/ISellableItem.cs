using System.Collections.Generic;
using UnityEngine;

namespace IntegratedGameplaySystem
{
    public interface ISellableItem
    {
        public Sprite Sprite { get; }
        public int MaxCount { get; }
        public int Money { get; }
    }
}
