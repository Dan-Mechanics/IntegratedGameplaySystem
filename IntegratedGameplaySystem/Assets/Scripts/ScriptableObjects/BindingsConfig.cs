using System.Collections.Generic;
using UnityEngine;
using System;

namespace IntegratedGameplaySystem
{
    /// <summary>
    /// Temp solution to TXT file vibes.
    /// </summary>
    [CreateAssetMenu(menuName = "ScriptableObjects/" + nameof(BindingsConfig), fileName = "New " + nameof(BindingsConfig))]
    public class BindingsConfig : ScriptableObject
    {
        public List<Binding> bindings;

        public List<Binding> GetBindings() 
        {
            bindings.ForEach(x => x.keyCode = (KeyCode)Enum.Parse(typeof(KeyCode), x.key));

            return bindings;
        }
    }
}