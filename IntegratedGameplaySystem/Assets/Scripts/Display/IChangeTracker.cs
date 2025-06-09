using System;

namespace IntegratedGameplaySystem
{
    /// <summary>
    /// Only limit of this is you cant have more than one changetracker
    /// per script but thats fine because structs and such.
    /// </summary>
    public interface IChangeTracker<T>
    {
        event Action<T> OnChange;
    }
}