using System;
using UnityEngine;

namespace IntegratedGameplaySystem
{
    /// <summary>
    /// NOTE: I haven't tested if this works yet.
    /// </summary>
    public interface IDestroyable 
    { 
        event Action<object, GameObject> OnDestroy;
        void Destroy();
    }
}