using System;
using System.Collections.Generic;
using UnityEngine;

namespace IntegratedGameplaySystem
{
    /// <summary>
    /// We could wrap a behaviour around this??
    /// Perhaps make this a service locatior? might be smart.
    /// </summary>
    [CreateAssetMenu(menuName = nameof(BaseBehaviour) + "/" + nameof(InputHandler), fileName = "New " + nameof(InputHandler))]
    public class InputHandler : BaseBehaviour
    {
        /// <summary>
        /// Ideally we load this in via config file.
        /// </summary>
        [SerializeField] private List<Binding> bindings = default;
        private readonly Dictionary<PlayerAction, InputEvents> conversion = new Dictionary<PlayerAction, InputEvents>();

        public override void Start()
        {
            base.Start();

            // Or we could generate them as they are needed, but this is a little smoother.
            for (int i = 0; i < Enum.GetValues(typeof(PlayerAction)).Length; i++)
            {
                conversion.Add((PlayerAction)i, new InputEvents());
            }
        }

        public void AddBinding(Binding binding) 
        {
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

        public override void Update()
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