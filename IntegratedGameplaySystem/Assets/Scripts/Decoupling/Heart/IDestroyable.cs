using System;
using UnityEngine;

namespace IntegratedGameplaySystem
{
    public interface IDestroyable 
    { 
        event Action<object, GameObject> OnDestroy;
        void Destroy();
    }
}