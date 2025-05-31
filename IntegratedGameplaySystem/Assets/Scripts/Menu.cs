using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace IntegratedGameplaySystem
{
    [CreateAssetMenu(menuName = "ScriptableObjects/" + nameof(Menu), fileName = "New " + nameof(Menu))]
    public class Menu : ScriptableObject, IScene, IDisposable
    {
        public GameObject canvas;
        public string nextScene;

        private Button button;

        public void Dispose()
        {
            button.onClick.RemoveListener(NextScene);
        }

        public List<object> GetGameBehaviours()
        {
            List<object> result = new List<object>();

            Transform canvas = Utils.SpawnPrefab(this.canvas).transform;
            button = canvas.GetComponentInChildren<Button>();
            button.onClick.AddListener(NextScene);

            result.Add(this);

            return result;
        }

        /// <summary>
        /// Repeatign fuck this shit.
        /// </summary>
        private void NextScene() 
        {
            SceneManager.LoadScene(nextScene);
        }
    }
}