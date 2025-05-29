using System;

namespace IntegratedGameplaySystem
{
    public class InputSource
    {
        public bool IsPressed { get; private set; }
        public Action OnDown;
        public Action OnUp;
        public Action<bool> OnChange;

        public InputSource()
        {
            OnDown += Compress;
            OnUp += Release;
        }

        private void Compress() 
        {
            IsPressed = true;
            OnChange?.Invoke(IsPressed);
        }

        private void Release()
        {
            IsPressed = false;
            OnChange?.Invoke(IsPressed);
        }
    }
}