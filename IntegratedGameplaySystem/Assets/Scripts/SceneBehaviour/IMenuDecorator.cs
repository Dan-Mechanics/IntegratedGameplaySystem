using System.Collections.Generic;
using UnityEngine;

namespace IntegratedGameplaySystem
{
    // This is to allow a little more flexiblity for non-scriptableobject.
    /*public interface IScene 
    {
        List<object> GetSceneComponents();
    }*/

    public interface IMenuDecorator
    {
        void Decorate(List<object> components, Transform canvas);
    }
}