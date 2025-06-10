using System.Collections.Generic;

namespace IntegratedGameplaySystem
{
    /// <summary>
    /// You can easily swap out or build on 
    /// the input rules.
    /// </summary>
    public class RestrictiveBindingRules : INewBindingRules
    {
        private readonly INewBindingRules chillBindingRules = new ChillBindingRules();
        
        public bool AllowBinding(List<Binding> alreadyBound, Binding newBinding)
        {
            if (!chillBindingRules.AllowBinding(alreadyBound, newBinding))
                return false;

            // If we found duplicate playerAction, dont allow.
            if (alreadyBound.Find(x => x.playerAction == newBinding.playerAction) != null)
                return false;

            // If we found duplicate keyCode, dont allow.
            if (alreadyBound.Find(x => x.keyCode == newBinding.keyCode) != null)
                return false;

            return true;
        }
    }
}