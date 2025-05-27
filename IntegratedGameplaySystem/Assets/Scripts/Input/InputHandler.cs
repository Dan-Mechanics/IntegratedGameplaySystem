using System;
using System.Collections.Generic;
using UnityEngine;

namespace IntegratedGameplaySystem
{
    /// <summary>
    /// Note to self: this is shti and needs to be fixed !!!
    /// It doesnt make sense because we can add and remove bindings dynamically but then if something is bound to it you have uno issue.
    /// No, it still makes sense.
    /// </summary>
    public class InputHandler : IInputService, IUpdatable
    {
        private readonly List<Binding> bindings = new();
        private readonly Dictionary<PlayerAction, InputSource> conversion;
        private readonly IBindingRule[] rules;

        public InputHandler(IBindingRule[] rules)
        {
            this.rules = rules;

            for (int i = 0; i < Enum.GetValues(typeof(PlayerAction)).Length; i++)
            {
                conversion.Add((PlayerAction)i, new InputSource());
            }
        }

        /// <summary>
        /// It would be cool if these rules worked with a func<> type beat.
        /// </summary>
        public void AddBinding(Binding binding)
        {
            foreach (IBindingRule rule in rules)
            {
                if (!rule.AllowBinding(bindings, binding))
                    return;
            }

            bindings.Add(binding);
            Debug.Log($"added binding {binding.key}");
        }

        public void RemoveBinding(Binding binding) => bindings.Remove(binding);

        public void RemoveByKey(KeyCode key)
        {
            for (int i = bindings.Count - 1; i >= 0; i--)
            {
                if (bindings[i].key == key)
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
            foreach (var binding in bindings)
            {
                if (!conversion.ContainsKey(binding.playerAction))
                    continue;

                conversion[binding.playerAction].SetPressed(Input.GetKey(binding.key));
            }
        }

        public InputSource GetInputSource(PlayerAction playerAction) => conversion[playerAction];

        public interface IBindingRule 
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

        public class DisallowMultiblePlayerAction : IBindingRule 
        {
            public bool AllowBinding(List<Binding> bindings, Binding binding)
            {
                // we dont find it.
                return binding.playerAction == PlayerAction.None ||
                    bindings.Find(x => x.playerAction == binding.playerAction) == null;
            }
        }

        public class DisallowMultibleKeyCode : IBindingRule
        {
            public bool AllowBinding(List<Binding> bindings, Binding binding)
            {
                // we dont find it.
                return binding.key == KeyCode.None || 
                    bindings.Find(x => x.key == binding.key) == null;
            }
        }
    }
}