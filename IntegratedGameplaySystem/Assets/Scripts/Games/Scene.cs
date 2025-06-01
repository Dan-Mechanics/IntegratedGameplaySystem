using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace IntegratedGameplaySystem
{
    [CreateAssetMenu(menuName = "ScriptableObjects/" + nameof(Scene), fileName = "New " + nameof(Scene))]
    public abstract class Scene : ScriptableObject, IScene, IDisposable
    {
        public string nextScene;
        
        public virtual List<object> GetGame()
        {
            List<object> behaviours = new();
            EventManager.AddListener(Occasion.GAME_OVER, NextScene);

            return behaviours;
        }

        protected void NextScene()
        {
            SceneManager.LoadScene(nextScene);
        }
        
        public virtual void Dispose()
        {
            EventManager.RemoveListener(Occasion.GAME_OVER, NextScene);
        }
    }
}