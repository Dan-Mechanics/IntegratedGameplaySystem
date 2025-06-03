using System;
using UnityEngine;

namespace IntegratedGameplaySystem
{
    public interface IStartable { void Start(); }
    public interface IUpdatable { void Update(); }
    public interface IDisposable { void Dispose(); }
    public interface IFixedUpdatable { void FixedUpdate(); }
    public interface ILateFixedUpdatable { void LateFixedUpdate(); }
    public interface IDestroyable { event Action<object, GameObject> OnDestroy; void Destroy(); }
}