using System.Collections.Generic;
using UnityEngine;

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

        /// <summary>
        /// Setup the sceen and provide common services between scenes.
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