using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace IntegratedGameplaySystem
{
    [CreateAssetMenu(menuName = "ScriptableObjects/" + nameof(Menu), fileName = "New " + nameof(Menu))]
    public class Menu : Scene
    {
        public GameObject canvas;
        //public string nextScene;

        private Button button;

        public override void Dispose()
        {
            base.Dispose();
            button.onClick.RemoveListener(NextScene);
        }

        public override List<object> GetGameBehaviours()
        {
            List<object> behaviours = new List<object>();

            Transform canvas = Utils.SpawnPrefab(this.canvas).transform;
            button = canvas.GetComponentInChildren<Button>();
            button.onClick.AddListener(NextScene);

            //result.Add(this);

            behaviours.AddRange(base.GetGameBehaviours());

            return behaviours;
        }
    }
}