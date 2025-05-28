using System.Collections.Generic;
using UnityEngine;

namespace IntegratedGameplaySystem
{
    public class DefaultBindingRules : INewBindingRule
    {
        public bool AllowBinding(List<Binding> bindings, Binding binding)
        {
            if (binding.playerAction == PlayerAction.None || binding.keyCode == KeyCode.None)
                return true;

            // If we found doubles, dont allow.
            if (bindings.Find(x => x.playerAction == binding.playerAction) != null)
                return false;

            // If we found doubles, dont allow.
            if (bindings.Find(x => x.keyCode == binding.keyCode) != null)
                return false;

            return true;
        }
    }
}