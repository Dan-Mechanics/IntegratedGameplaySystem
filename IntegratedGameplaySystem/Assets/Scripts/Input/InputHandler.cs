using System;
using System.Collections.Generic;
using UnityEngine;

namespace IntegratedGameplaySystem
{
    public class InputHandler : IInputService
    {
        private readonly List<Binding> bindings = new();
        private readonly Dictionary<PlayerAction, InputSource> conversion = new();
        private readonly INewBindingRules newBindingRules;

        public InputHandler(INewBindingRules newBindingRules, IBindingsSource source)
        {
            this.newBindingRules = newBindingRules;
            source.GetBindings().ForEach(x => AddBinding(x));

            for (int i = 0; i < Enum.GetValues(typeof(PlayerAction)).Length; i++)
            {
                conversion.Add((PlayerAction)i, new InputSource());
            }
        }

        public void AddBinding(Binding binding)
        {
            if (!newBindingRules.AllowBinding(bindings, binding))
                return;

            bindings.Add(binding);
        }

        public void RemoveBinding(Binding binding) => bindings.Remove(binding);

        public void RemoveByKey(KeyCode key)
        {
            for (int i = bindings.Count - 1; i >= 0; i--)
            {
                if (bindings[i].keyCode == key)
                    bindings.RemoveAt(i);
            }
        }

        public void RemoveByPlayerAction(PlayerAction playerAction)
        {
            for (int i = bindings.Count - 1; i >= 0; i--)
            {
                if (bindings[i].playerAction == playerAction)
                    bindings.RemoveAt(i);
            }
        }

        public void Update()
        {
            for (int i = 0; i < bindings.Count; i++)
            {
                if (Input.GetKeyDown(bindings[i].keyCode))
                    conversion[bindings[i].playerAction].onDown?.Invoke();

                if (Input.GetKeyUp(bindings[i].keyCode))
                    conversion[bindings[i].playerAction].onUp?.Invoke();
            }
        }
         
        public InputSource GetInputSource(PlayerAction playerAction) => conversion[playerAction];

        /// <summary>
        /// Add this because we want all the listeners to be reset back to normal.
        /// </summary>
        public void Dispose()
        {
            foreach (var pair in conversion)
            {
                pair.Value.onUp?.Invoke();
            }
        }
    }
}