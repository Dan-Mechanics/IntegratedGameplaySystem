using System;
using UnityEngine;
using UnityEngine.UI;

namespace IntegratedGameplaySystem
{
    public interface IChangeTracker<T>
    {
        public event Action<T> OnChange;
    }

    public interface IChangeTracker<T1, T2>
    {
        public event Action<T1, T2> OnChange;
    }
}