using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace IntegratedGameplaySystem
{
    public interface INewBindingRule
    {
        // This is where you can define the rules for which bindings are allowed.

        // Right now: A --> PrimaryFire AND A --> SecondaryFire
        // but not A AND B --> PrimaryFire.

        // Or we can have it that only one key is allowed to do one thing
        // but then multible keys can point to the same action still.

        // Or we have it that one key does one thing and an action can only have
        // one key associated with it, but what's the fun in that ?
        bool AllowBinding(List<Binding> bindings, Binding binding);
    }
}