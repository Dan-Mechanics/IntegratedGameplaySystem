using UnityEngine;
using UnityEngine.SceneManagement;
using System;

namespace IntegratedGameplaySystem
{
    public class TestingFeatures : IStartable, IDisposable
    {
        private readonly IInputService inputService;

        public TestingFeatures()
        {
            inputService = ServiceLocator<IInputService>.Locate();
        }

        public void Start()
        {
            inputService.GetInputSource(PlayerAction.Reload).onDown += Reload;
            inputService.GetInputSource(PlayerAction.Escape).onDown += Quit;
        }

        public void Dispose()
        {
            inputService.GetInputSource(PlayerAction.Reload).onDown -= Reload;
            inputService.GetInputSource(PlayerAction.Escape).onDown -= Quit;
        }

        private void Reload()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        private void Quit() => Application.Quit();
    }
}