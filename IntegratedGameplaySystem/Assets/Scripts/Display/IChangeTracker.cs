using System;

namespace IntegratedGameplaySystem
{
    /// <summary>
    /// Only limit of this is you can't have more than one IChangeTracker
    /// per script but thats fine because structs and such. It also
    /// encourages the scripts to be responsbile for the changes of 
    /// fewer things which is good.
    /// </summary>
    public interface IChangeTracker<T>
    {
        event Action<T> OnChange;
    }
}