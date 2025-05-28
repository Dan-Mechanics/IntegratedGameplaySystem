using System;
using System.Collections.Generic;
using UnityEngine;

namespace IntegratedGameplaySystem
{
    /// <summary>
    /// This name sucks.
    /// </summary>
    public class DisallowMultibleKeyCode : IBindingRule
    {
        public bool AllowBinding(List<Binding> bindings, Binding binding)
        {
            // we dont find it.
            return binding.keyCode == KeyCode.None ||
                bindings.Find(x => x.keyCode == binding.keyCode) == null;
        }
    }
}