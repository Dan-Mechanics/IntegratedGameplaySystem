using System;
using UnityEngine;
using UnityEngine.UI;

namespace IntegratedGameplaySystem
{
    /// <summary>
    /// Only limit of this is you cant have more than one changetracker
    /// per script but thats fine because structs and such.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IChangeTracker<T>
    {
        public event Action<T> OnChange;
    }
}