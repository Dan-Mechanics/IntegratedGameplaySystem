using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace IntegratedGameplaySystem
{
    /// <summary>
    /// Basically add a billion interfaces here.
    /// 
    /// Perhaps rename start to setup for swag points.
    /// </summary>
    public interface IStartable { void Start(); }
    public interface IUpdatable { void Update(); }
    public interface IDisposable { void Dispose(); }
    public interface IFixedUpdatable { void FixedUpdate(); }
    public interface ILateFixedUpdatable { void LateFixedUpdate(); }

    public interface IAssetService
    {
        T FindAsset<T>(string name) where T : Object;
    }
}