using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace IntegratedGameplaySystem
{
    [CreateAssetMenu(menuName = "ScriptableObjects/" + nameof(Menu), fileName = "New " + nameof(Menu))]
    public class Menu : SceneBehaviour
    {
        public GameObject canvasPrefab;
        public Object[] decorators;

        /// <summary>
        /// We mustk keep this in memory to dispose.
        /// </summary>
        private Button nextSceneButton;

        public override void Start()
        {
            base.Start();

            for (int i = 0; i < decorators.Length; i++)
            {
                if (decorators[i] is IStartable startable)
                    startable.Start();
            }
        }

        public override List<object> GetSceneComponents()
        {
            List<object> components = base.GetSceneComponents();

            Transform canvas = Utils.SpawnPrefab(canvasPrefab).transform;
            nextSceneButton = canvas.GetComponentInChildren<Button>();
            nextSceneButton.onClick.AddListener(sceneHandler.GoNextScene);

            for (int i = 0; i < decorators.Length; i++)
            {
                if (decorators[i] is IMenuDecorator decorator)
                    decorator.Decorate(components, canvas);
            }

            return components;
        }

        public override void Dispose()
        {
            nextSceneButton.onClick.RemoveListener(sceneHandler.GoNextScene);

            for (int i = 0; i < decorators.Length; i++)
            {
                if (decorators[i] is IDisposable disposable)
                    disposable.Dispose();
            }
        }
    }
}