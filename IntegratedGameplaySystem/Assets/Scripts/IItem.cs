using System.Collections.Generic;
using UnityEngine;

namespace IntegratedGameplaySystem
{
    public interface IItem
    {
        public Sprite Sprite { get; }
        public int MaxCount { get; }
        public int Money { get; }
    }
}
