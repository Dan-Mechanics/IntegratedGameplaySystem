using System;

namespace IntegratedGameplaySystem
{
    public class InputSource
    {
        public bool IsPressed { get; private set; }
        public event Action<bool> OnChange;

        public Action onDown;
        public Action onUp;

        public InputSource()
        {
            onDown += Compress;
            onUp += Release;
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