using System;
using UnityEngine;

namespace IntegratedGameplaySystem
{
    public interface IPurchasable : IInteractable, IHoverable
    {
        public event Func<int, bool> OnCanBuy;
        public event Action OnBuy;
    }

    public interface IGradeUp : IStartable, IDisposable
    {
        public IPurchasable Purchasable { get; }
    }
}