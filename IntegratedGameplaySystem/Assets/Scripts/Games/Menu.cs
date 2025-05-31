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
        protected Transform canvasTrans;

        private Button button;
        
        public override void Dispose()
        {
            base.Dispose();
            button.onClick.RemoveListener(NextScene);
        }

        public override List<object> GetGame()
        {
            List<object> behaviours = new List<object>();

            canvasTrans = Utils.SpawnPrefab(canvas).transform;
            button = canvasTrans.GetComponentInChildren<Button>();
            button.onClick.AddListener(NextScene);
          //  Debug.Log(button);
            //result.Add(this);

            behaviours.AddRange(base.GetGame());

            return behaviours;
        }
    }
}