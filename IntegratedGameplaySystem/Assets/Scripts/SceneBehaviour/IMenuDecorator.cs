using System.Collections.Generic;
using UnityEngine;

namespace IntegratedGameplaySystem
{
    public interface IMenuDecorator
    {
        void Decorate(List<object> components, Transform canvas);
    }
}