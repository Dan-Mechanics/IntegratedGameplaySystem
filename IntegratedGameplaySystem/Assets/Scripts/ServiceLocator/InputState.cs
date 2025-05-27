using System;

namespace IntegratedGameplaySystem
{
    public class InputState
    {
        public Action OnDown;
        public Action OnUp;
        public Action<bool> OnChange;

        public bool isPressed;

        /// <summary>
        /// They call me the coder.
        /// </summary>
        public InputState()
        {
            OnChange += (bool value) => isPressed = value;
        }
    }
}