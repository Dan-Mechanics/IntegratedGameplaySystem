using System;

namespace IntegratedGameplaySystem
{
    public class InputSource
    {
        public event Action OnDown;
        public event Action OnUp;
        public event Action<bool> OnChange;
        public bool IsPressed { get; private set; }

        public void SetPressed(bool newPressed) 
        {
            if (IsPressed != newPressed)
                OnChange?.Invoke(newPressed);

            if (!IsPressed && newPressed)
                OnDown?.Invoke();

            if (IsPressed && !newPressed)
                OnUp?.Invoke();

            IsPressed = newPressed;
        }
    }
}