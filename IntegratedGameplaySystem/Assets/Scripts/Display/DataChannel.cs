using System;
using System.Collections.Generic;

namespace IntegratedGameplaySystem 
{ 
    /// <summary>
    /// We wan't to channel T1 into T2.
    /// </summary>
    public class DataChannel<T1, T2> : IDisposable
    {
        public event Action<T1, T2> OnChange;
        public T2 ui;
        private readonly IChangeTracker<T1> changeTracker;

        public DataChannel(IChangeTracker<T1> changeTracker, List<IDisposable> disposables)
        {
            this.changeTracker = changeTracker;
            changeTracker.OnChange += Combine;

            disposables.Add(this);
        }

        private void Combine(T1 a) => OnChange?.Invoke(a, ui);
        public void Dispose() => changeTracker.OnChange -= Combine;
    }
}