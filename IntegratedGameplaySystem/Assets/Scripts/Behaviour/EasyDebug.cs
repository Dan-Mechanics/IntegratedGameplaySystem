using UnityEngine;
using UnityEngine.SceneManagement;

namespace IntegratedGameplaySystem
{
    public class EasyDebug : IStartable, IDisposable
    {
        private readonly IInputService inputService;

        public event System.Action<object> OnDispose;

        public EasyDebug()
        {
            inputService = ServiceLocator<IInputService>.Locate();
        }

        public void Start()
        {
            inputService.GetInputSource(PlayerAction.Reload).OnDown += Reload;
            inputService.GetInputSource(PlayerAction.Escape).OnDown += Quit;
        }

        public void Dispose()
        {
            inputService.GetInputSource(PlayerAction.Reload).OnDown -= Reload;
            inputService.GetInputSource(PlayerAction.Escape).OnDown -= Quit;
            OnDispose?.Invoke(this);
        }

        private void Reload()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        private void Quit() => Application.Quit();
    }
}