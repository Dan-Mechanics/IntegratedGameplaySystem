using System;
using System.Collections.Generic;
using UnityEngine;

namespace IntegratedGameplaySystem
{
    public class BindingsConfig : IStartable
    {
        /// <summary>
        /// CONFIG!!
        /// </summary>
        public List<Binding> bindings;



        private InputHandler inputHandler;

        public BindingsConfig(string path)
        {
        }

        /// <summary>
        /// https://stackoverflow.com/questions/16100/convert-a-string-to-an-enum-in-c-sharp
        /// </summary>
        public void Start()
        {
            //inputHandler = new InputHandler();

            /*for (int i = 0; i < bindings.Count; i++)
            {
                bindings[i].key = (KeyCode)Enum.Parse(typeof(KeyCode), bindings[i].key);
                inputHandler.AddBinding(bindings[i]);
            }*/

            ServiceLocator<IInputService>.Provide(inputHandler);
        }
    }
}