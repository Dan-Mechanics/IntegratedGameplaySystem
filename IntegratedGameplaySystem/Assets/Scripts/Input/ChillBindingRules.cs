using System.Collections.Generic;
using UnityEngine;

namespace IntegratedGameplaySystem
{
    /// <summary>
    /// Consider not having interface usage here?
    /// </summary>
    public class ChillBindingRules : INewBindingRules
    {
        public bool AllowBinding(List<Binding> alreadyBound, Binding newBinding)
        {
            if (newBinding.playerAction == PlayerAction.None || newBinding.keyCode == KeyCode.None)
                return false;

            if (alreadyBound.Find(x => x.Equals(newBinding)) != null)
                return false;

            return true;
        }
    }
}