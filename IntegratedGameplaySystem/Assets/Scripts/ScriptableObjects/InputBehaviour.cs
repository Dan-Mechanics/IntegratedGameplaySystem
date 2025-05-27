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
        /// <summary>
        /// CONFIG!!
        /// </summary>
        public List<Binding> bindings;



        private InputHandler inputHandler;

        /// <summary>
        /// https://stackoverflow.com/questions/16100/convert-a-string-to-an-enum-in-c-sharp
        /// </summary>
        public override void Start()
        {
            base.Start();
            inputHandler = new InputHandler();

            for (int i = 0; i < bindings.Count; i++)
            {
                bindings[i].keyCode = (KeyCode)Enum.Parse(typeof(KeyCode), bindings[i].key);
                inputHandler.AddBinding(bindings[i]);
            }

            ServiceLocator<IInputService>.Provide(inputHandler);
        }

        public override void Update()
        {
            base.Update();
            inputHandler.Update();
        }

        //public InputEvents GetAction(PlayerAction playerAction) => inputHandler?.GetAction(playerAction);
    }
}