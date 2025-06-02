//using System;
using System.Collections.Generic;
using UnityEngine;

namespace IntegratedGameplaySystem
{
    /// <summary>
    /// SINGLE MONOBEHAVIOUR HERE !!
    /// </summary>
    public class GameManager : MonoBehaviour 
    {
        [SerializeField] private Object scene = default;
        [SerializeField] private SceneSetup sceneSetup = default;
        [SerializeField] private List<GameObject> scenePrefabs = default;
        [SerializeField] private Assets assets = default;

        private readonly Heart heart = new Heart();

        private void Start() => Setup();
        private void Update() => heart.Update();
        private void OnDisable() => heart.Dispose();
        private void OnApplicationQuit() => EventManager.RaiseEvent(Occasion.CLOSE_GAME);

        /// <summary>
        /// Chat I think this project is a little overengineerd but thats fun.
        /// Consider moving this setup to an inhertied thng.
        /// </summary>
        private void Setup()
        {
            sceneSetup.Start();
            scenePrefabs.ForEach(x => Utils.SpawnPrefab(x));

            ServiceLocator<IAssetService>.Provide(assets);
            ServiceLocator<IWorldService>.Provide(null);

            if (ServiceLocator<IInputService>.Locate() == null)
                ServiceLocator<IInputService>.Provide(new InputHandler(new ChillBindingRules(), new ConfigTextFile()));

            if (scene == null || scene is not IScene foundScene)
            {
                Debug.LogError("Please assign a valid IScene scene.");
                return;
            }

            Debug.Log($"loading {scene.name.ToUpper()}");

            // This will throw an error if the scene is not an IScene.
            // this is on purpouse because then i assigned the wrong thing.
            List<object> behaviours = foundScene.GetSceneBehaviours();
            behaviours.Add(scene);

            behaviours.Add(ServiceLocator<IInputService>.Locate());
            behaviours.Add(new TestingFeatures());

            heart.Setup(behaviours);
        }
    }
}