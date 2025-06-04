using System;
using UnityEngine.SceneManagement;

namespace IntegratedGameplaySystem
{
    [Serializable]
    public class NextSceneHandler : IStartable, IDisposable
    {
        public string nextScene;

        public void Start()
        {
            EventManager.AddListener(Occasion.GameOver, GoNextScene);
        }

        public void GoNextScene()
        {
            SceneManager.LoadScene(nextScene);
        }

        public void Dispose()
        {
            EventManager.RemoveListener(Occasion.GameOver, GoNextScene);
        }
    }
}