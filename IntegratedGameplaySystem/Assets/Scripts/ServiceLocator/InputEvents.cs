using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace IntegratedGameplaySystem
{
    public class InputEvents
    {
        public Action OnDown;
        public Action OnUp;
        public Action<bool> OnChange;

        public bool isPressed;

        /// <summary>
        /// They call me the coder.
        /// </summary>
        public InputEvents()
        {
            OnChange += (bool value) => isPressed = value;
        }
    }
}