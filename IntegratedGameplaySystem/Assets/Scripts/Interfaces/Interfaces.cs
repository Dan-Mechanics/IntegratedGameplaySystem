using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace IntegratedGameplaySystem
{
    /// <summary>
    /// Basically add a billion interfaces here.
    /// 
    /// Perhaps rename start to setup for swag points.
    /// </summary>
    public interface IStartable { void Start(); }
    public interface IUpdatable { void Update(); }
    public interface IDisposable
    {
        void Dispose();
    }

    /// <summary>
    /// THINK ABOUT GAMEWORLD BULLSHIT !!.
    /// </summary>
    public interface IDestroyable 
    {
        event Action<object> OnDestroy;
        void Destroy();
    }

    public interface IFixedUpdatable { void FixedUpdate(); }
    public interface ILateFixedUpdatable { void LateFixedUpdate(); }


    /// <summary>
    /// Or this could be addressable, you get the point.
    /// </summary>
    public interface IAssetService
    {
        GameObject GetPlayer();
        RaycastData GetInteractionRaycast();
        MovementSettings GetMovementSettings();
        GroundedConfiguration GetGroundedConfig();
        BindingsConfig GetBindingsConfig();
    }
}