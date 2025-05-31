using System.Collections.Generic;
using UnityEngine;

namespace IntegratedGameplaySystem
{
    public interface IScene 
    {
        List<object> GetGameBehaviours();
    }
}