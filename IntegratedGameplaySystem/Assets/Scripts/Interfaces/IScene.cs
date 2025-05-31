using System.Collections.Generic;
using UnityEngine;

namespace IntegratedGameplaySystem
{
    /// <summary>
    /// This is to allow a little more flexiblity for non-scriptableobject.
    /// </summary>
    public interface IScene 
    {
        List<object> GetGame();
    }
}