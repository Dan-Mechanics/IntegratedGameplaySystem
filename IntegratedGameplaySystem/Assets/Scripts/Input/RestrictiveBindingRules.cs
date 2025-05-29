using System.Collections.Generic;
using UnityEngine;

namespace IntegratedGameplaySystem
{
    /// <summary>
    /// The cool thing is you can change these rules with
    /// different classes right.
    /// </summary>
    public class RestrictiveBindingRules : INewBindingRules
    {
        private readonly ChillBindingRules chillBindingRules = new();
        
        public bool AllowBinding(List<Binding> alreadyBound, Binding newBinding)
        {
            if (!chillBindingRules.AllowBinding(alreadyBound, newBinding))
                return false;

            // If we found doubles, dont allow.
            if (alreadyBound.Find(x => x.playerAction == newBinding.playerAction) != null)
                return false;

            if (alreadyBound.Find(x => x.keyCode == newBinding.keyCode) != null)
                return false;

            return true;
        }
    }
}