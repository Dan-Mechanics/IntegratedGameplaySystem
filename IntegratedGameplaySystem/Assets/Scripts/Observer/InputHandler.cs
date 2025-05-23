using System;
using System.Collections.Generic;
using UnityEngine;

namespace DesignPatterns
{
    public class InputHandler
    {
        /// <summary>
        /// Ideally we load this in via config file.
        /// </summary>
        private readonly List<Binding> bindings;
        private readonly Dictionary<PlayerAction, InputEvents> conversion;

        public InputHandler(List<Binding> bindings = null)
        {
            this.bindings = bindings ?? new List<Binding>();
            conversion = new Dictionary<PlayerAction, InputEvents>();

            // Or we could generate them as they are needed, but this is a little smoother.
            for (int i = 0; i < Enum.GetValues(typeof(PlayerAction)).Length; i++)
            {
                conversion.Add((PlayerAction)i, new InputEvents());
            }
        }

        public void AddBinding(Binding binding) 
        {
            // This is where you can define the rules for which bindings are allowed.

            // Right now: A --> PrimaryFire AND A --> SecondaryFire
            // but not A AND B --> PrimaryFire.

            // Or we can have it that only one key is allowed to do one thing
            // but then multible keys can point to the same action still.

            // Or we have it that one key does one thing and an action can only have
            // one key associated with it, but what's the fun in that ?

            if (bindings.Find(x => x.playerAction == binding.playerAction) != null)
            {
                Debug.LogWarning("if (bindings.Find(x => x.playerAction == binding.playerAction) != null)");
                return;
            }

            bindings.Add(binding);
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

                if (Input.GetKeyDown(binding.key))
                {
                    conversion[binding.playerAction].OnDown?.Invoke();
                    conversion[binding.playerAction].OnChange?.Invoke(true);
                }

                if (Input.GetKeyUp(binding.key)) 
                {
                    conversion[binding.playerAction].OnUp?.Invoke();
                    conversion[binding.playerAction].OnChange?.Invoke(false);
                }
            }
        }

        public InputEvents GetInputEvents(PlayerAction playerAction) => conversion[playerAction];

        [Serializable]
        public class Binding
        {
            public KeyCode key;
            public PlayerAction playerAction;

            public Binding(KeyCode key, PlayerAction playerAction)
            {
                this.key = key;
                this.playerAction = playerAction;
            }
        }

        public class InputEvents 
        {
            public Action OnDown;
            public Action OnUp;
            public Action<bool> OnChange;
        }
    }

}