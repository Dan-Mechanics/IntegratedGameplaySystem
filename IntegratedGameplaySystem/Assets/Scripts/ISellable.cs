using System.Collections.Generic;
using UnityEngine;

namespace IntegratedGameplaySystem
{
    public interface ISellable
    {
        public Sprite Sprite { get; }
        public int MaxCount { get; }
        public int Money { get; }
    }

    public interface IInteractionChanger 
    {
        public LayerMask Mask { get; }
        public List<string> Tags { get;  }
    }
}
