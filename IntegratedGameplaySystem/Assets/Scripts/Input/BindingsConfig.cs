using System.Collections.Generic;
using UnityEngine;

namespace IntegratedGameplaySystem
{
    [CreateAssetMenu(menuName = "ScriptableObjects/" + nameof(BindingsConfig), fileName = "New " + nameof(BindingsConfig))]
    public class BindingsConfig : ScriptableObject, IBindingsSource
    {
        public List<Binding> bindings;

        public List<Binding> GetBindings() 
        {
            bindings.ForEach(x => x.ProcessKeyString());
            return bindings;
        }
    }
}