using System;
using UnityEngine.SceneManagement;

namespace IntegratedGameplaySystem
{
    [Serializable]
    public class SceneHandler : IStartable, IDisposable
    {
        public string nextScene;

        public void Start()
        {
            EventManager.AddListener(Occasion.GAME_OVER, GoNextScene);
        }

        public void GoNextScene()
        {
            SceneManager.LoadScene(nextScene);
        }

        public void Dispose()
        {
            EventManager.RemoveListener(Occasion.GAME_OVER, GoNextScene);
        }
    }
}