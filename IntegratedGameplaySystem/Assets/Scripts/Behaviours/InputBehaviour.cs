using System;
using System.Collections.Generic;
using UnityEngine;

namespace IntegratedGameplaySystem
{
    /// <summary>
    /// We could wrap a behaviour around this??
    /// Perhaps make this a service locatior? might be smart.
    /// </summary>
    [CreateAssetMenu(menuName = nameof(BaseBehaviour) + "/" + nameof(InputBehaviour), fileName = "New " + nameof(InputBehaviour))]
    public class InputBehaviour : BaseBehaviour
    {
        public List<InputHandler.Binding> bindings;
        public InputHandler inputHandler;

        public override void Start()
        {
            base.Start();
            inputHandler = new InputHandler();

            for (int i = 0; i < bindings.Count; i++)
            {
                bindings[i].keyCode = (KeyCode)Enum.Parse(typeof(KeyCode), bindings[i].key);
                inputHandler.AddBinding(bindings[i]);
            }
        }

        public override void Update()
        {
            base.Update();
            inputHandler.Update();
        }

        public InputHandler.InputEvents GetAction(PlayerAction playerAction) => inputHandler?.GetAction(playerAction);
    }
}