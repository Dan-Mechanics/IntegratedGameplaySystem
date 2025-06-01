using System.Collections.Generic;
using UnityEngine.SceneManagement;

namespace IntegratedGameplaySystem
{
    [System.Serializable]
    public class SceneHandler : IScene, IDisposable
    {
        public string nextScene;

        public List<object> GetSceneBehaviours()
        {
            List<object> behaviours = new();
            EventManager.AddListener(Occasion.GAME_OVER, GoNextScene);

            return behaviours;
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