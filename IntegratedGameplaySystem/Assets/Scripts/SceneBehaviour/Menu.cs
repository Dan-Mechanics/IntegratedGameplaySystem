using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace IntegratedGameplaySystem
{
    [CreateAssetMenu(menuName = "ScriptableObjects/" + nameof(Menu), fileName = "New " + nameof(Menu))]
    public class Menu : SceneBehaviour
    {
        //public NextSceneHandler sceneHandler;
        public GameObject canvasPrefab;
        public Object[] decorators;

        //private readonly List<IMenuDecorator> menuDecorators = new();

        /// <summary>
        /// We mustk keep this in memory to dispose.
        /// </summary>
        private Button nextSceneButton;

        public override void Start()
        {
            base.Start();

            for (int i = 0; i < decorators.Length; i++)
            {
                /*if (decorators[i] is IMenuDecorator decorator)
                    menuDecorators.Add(decorator);*/

                if (decorators[i] is IStartable startable)
                    startable.Start();
            }
        }

        public override List<object> GetSceneComponents()
        {
            //sceneHandler.Start();
            List<object> components = base.GetSceneComponents();

            Transform canvas = Utils.SpawnPrefab(canvasPrefab).transform;
            nextSceneButton = canvas.GetComponentInChildren<Button>();
            nextSceneButton.onClick.AddListener(sceneHandler.GoNextScene);

            //menuDecorators.ForEach(x => x.Decorate(components, canvas));

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

            // becuase i might want to add disposing features 
            // in the feature like more buttons right.
            /*foreach (var item in decorators)
            {
                if (decorators is not IDisposable disposable)
                    continue;

                disposable.Dispose();
            }*/
        }
    }
}