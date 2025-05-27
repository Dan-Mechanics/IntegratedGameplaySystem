using System;
using System.Collections.Generic;
using UnityEngine;

namespace IntegratedGameplaySystem
{
    public class DisallowMultiblePlayerAction : IBindingRule
    {
        public bool AllowBinding(List<Binding> bindings, Binding binding)
        {
            // we dont find it.
            return binding.playerAction == PlayerAction.None ||
                bindings.Find(x => x.playerAction == binding.playerAction) == null;
        }
    }
}