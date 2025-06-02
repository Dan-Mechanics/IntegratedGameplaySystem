using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace IntegratedGameplaySystem
{
    [CreateAssetMenu(menuName = "ScriptableObjects/" + nameof(Menu), fileName = "New " + nameof(Menu))]
    public class Menu : ScriptableObject, IScene, IDisposable
    {
        public SceneHandler sceneHandler;
        public GameObject canvasPrefab;
        public List<Object> decorators;

        /// <summary>
        /// We mustk keep this in memory to dispose.
        /// </summary>
        private Button nextSceneButton;
        
        public List<object> GetSceneBehaviours()
        {
            sceneHandler.Start();
            List<object> behaviours = new List<object>();

            Transform canvas = Utils.SpawnPrefab(canvasPrefab).transform;
            nextSceneButton = canvas.GetComponentInChildren<Button>();
            nextSceneButton.onClick.AddListener(sceneHandler.GoNextScene);

            decorators.ForEach(x => TryDecorate(x, behaviours, canvas));

            return behaviours;
        }

        private void TryDecorate(Object obj, List<object> behaviours, Transform canvas) 
        {
            if (obj is not IMenuDecorator dec)
                return;

            dec.Decorate(behaviours, canvas);
        }

        public void Dispose()
        {
            sceneHandler.Dispose();
            nextSceneButton.onClick.RemoveListener(sceneHandler.GoNextScene);

            // becuase i might want to add disposing features 
            // in the feature like more buttons right.
            foreach (var item in decorators)
            {
                if (decorators is not IDisposable disposable)
                    continue;

                disposable.Dispose();
            }
        }
    }
}