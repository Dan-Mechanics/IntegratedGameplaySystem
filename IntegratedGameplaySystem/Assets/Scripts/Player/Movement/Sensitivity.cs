using System;

namespace IntegratedGameplaySystem
{
    public class Sensitivity : IStartable, IDisposable, IChangeTracker<float>
    {
        public const float DEFAULT_SENS = 0.33f;
        public event Action<float> OnChange;
        public float Value => value;
        private readonly IInputService inputService;
        private float value;

        public Sensitivity()
        {
            inputService = ServiceLocator<IInputService>.Locate();
        }

        public void Start()
        {
            value = DEFAULT_SENS;
            OnChange?.Invoke(value);
            
            inputService.GetInputSource(PlayerAction.Up).onDown += Double;
            inputService.GetInputSource(PlayerAction.Down).onDown += Half;
        }

        public void Dispose()
        {
            inputService.GetInputSource(PlayerAction.Up).onDown -= Double;
            inputService.GetInputSource(PlayerAction.Down).onDown -= Half;
        }

        private void Half() 
        {
            value /= 2f;
            OnChange?.Invoke(value);
        }

        private void Double() 
        {
            value *= 2f;
            OnChange?.Invoke(value);
        }
    }
}