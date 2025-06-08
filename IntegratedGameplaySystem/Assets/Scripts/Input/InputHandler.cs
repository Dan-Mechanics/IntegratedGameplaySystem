using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace IntegratedGameplaySystem
{
    /// <summary>
    /// Note to self: this is shti and needs to be fixed !!!
    /// It doesnt make sense because we can add and remove bindings dynamically but then if something is bound to it you have uno issue.
    /// No, it still makes sense.
    /// </summary>
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

        /// <summary>
        /// It would be cool if these rules worked with a func<> type beat.
        /// But for now i like the interface.
        /// </summary>
        public void AddBinding(Binding binding)
        {
            if (!newBindingRules.AllowBinding(bindings, binding))
                return;

            bindings.Add(binding);
            //Debug.Log($"added binding {binding}");
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
        /// Because we want all the stuff to reset.
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