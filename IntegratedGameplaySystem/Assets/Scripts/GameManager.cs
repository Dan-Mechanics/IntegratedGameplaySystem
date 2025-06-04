using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

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

        private void Start() => Setup();
        private void Update() => heart.Update();
        private void OnDisable() => heart.Dispose();
        private void OnApplicationQuit() => EventManager.RaiseEvent(Occasion.CloseGame);
        private void OnValidate() => EditorSceneManager.sceneOpened += SelectInHierarchy;

        /// <summary>
        /// This is just a little side quest.
        /// IDK how this works but it does.
        /// </summary>
        private void SelectInHierarchy(Scene scene, OpenSceneMode mode)
        {
            // Idk why this needs to be in here but otherwise 
            // it throws an error sooooo.
            if (this != null && !Selection.objects.Contains(gameObject))
                Selection.objects = new Object[] { gameObject };

            EditorSceneManager.sceneOpened -= SelectInHierarchy;
        }

        /// <summary>
        /// NOTE: this code is a little over-abstracted but that's part of the learning !
        /// </summary>
        private void Setup()
        {
            sceneSetup.Setup();
            scenePrefabs.ForEach(x => Utils.SpawnPrefab(x));

            ServiceLocator<IAssetService>.Provide(inspectorAssets);

            IWorldService worldService = ServiceLocator<IWorldService>.Locate();
            if (worldService == null)
            {
                ServiceLocator<IWorldService>.Provide(new GameWorld());
            }
            else
            {
                worldService.Reset();
            }

            IInputService inputService = ServiceLocator<IInputService>.Locate();
            if (inputService == null)
            {
                inputService = new InputHandler(new ChillBindingRules(), new ConfigTextFile());
                ServiceLocator<IInputService>.Provide(inputService);
            }

            if (sceneBehaviour == null)
            {
                Debug.LogError($"please assign a valid {nameof(sceneBehaviour)}.");
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