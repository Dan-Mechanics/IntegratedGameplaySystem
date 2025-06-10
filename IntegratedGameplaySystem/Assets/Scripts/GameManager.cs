using System.Collections.Generic;
using UnityEngine;

/*using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;*/

namespace IntegratedGameplaySystem
{
    /// <summary>
    /// SINGLE MONOBEHAVIOUR HERE !!
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private SceneBehaviour sceneBehaviour = default;
        [SerializeField] private SceneSetup sceneSetup = default;
        [SerializeField] private List<GameObject> scenePrefabs = default;
        [SerializeField] private InspectorAssets inspectorAssets = default;

        private readonly Heart heart = new Heart();

        private void Start() => SetupScene();
        private void Update() => heart.Update();
        private void OnDisable() => heart.Dispose();
        private void OnApplicationQuit() => EventManager.RaiseEvent(Occasion.CloseGame);

        /*private void OnValidate() => EditorSceneManager.sceneOpened += SelectInHierarchy;
        private void SelectInHierarchy(Scene scene, OpenSceneMode mode)
        {
            // Idk why this needs to be in here but otherwise 
            // it throws an error sooooo.
            if (this != null && !Selection.objects.Contains(gameObject))
                Selection.objects = new Object[] { gameObject };

            EditorSceneManager.sceneOpened -= SelectInHierarchy;
        }*/

        /// <summary>
        /// NOTE: this code is a little over-abstracted but that's part of the learning !
        /// </summary>
        private void SetupScene()
        {
            sceneSetup.Setup();
            scenePrefabs.ForEach(x => Utils.SpawnPrefab(x));

            ServiceLocator<IAssetService>.Provide(inspectorAssets);

            if (!ServiceLocator<IInputService>.HasBeenProvided())
            {
                ServiceLocator<IWorldService>.Provide(new GameWorld());
            }
            else
            {
                ServiceLocator<IWorldService>.Locate().Reset();
            }

            IInputService inputService;
            if (!ServiceLocator<IInputService>.HasBeenProvided())
            {
                inputService = new InputHandler(new ChillBindingRules(), new ConfigTextFile());
                ServiceLocator<IInputService>.Provide(inputService);
            }
            else 
            {
                inputService = ServiceLocator<IInputService>.Locate();
            }

            if (sceneBehaviour == null)
            {
                Debug.LogError($"please assign a valid {nameof(SceneBehaviour)}.");
                return;
            }

            Debug.Log($"loading {sceneBehaviour.name.ToUpper()}");

            List<object> components = sceneBehaviour.GetSceneComponents();

            components.Add(sceneBehaviour);
            components.Add(inputService);
            components.Add(new TestingFeatures());

            heart.Setup(components);
        }
    }
}