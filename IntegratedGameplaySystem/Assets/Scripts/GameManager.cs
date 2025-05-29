using System;
using System.Collections.Generic;
using UnityEngine;

namespace IntegratedGameplaySystem
{
    /// <summary>
    /// SINGLE MONOBEHAVIOUR HERE !!
    /// </summary>
    public class GameManager : MonoBehaviour 
    {
        public const string PLAYER = "player";
        public const string PLANT = "plant";
        public const string PLAYER_SETTINGS = "player_settings";
        public const string BINDINGS = "config";
        public const string INTERACT = "interact_raycast";

        [SerializeField] private SceneSetup sceneSetup = default;
        [SerializeField] private List<GameObject> scenePrefabs = default;
        [SerializeField] private Assets assets = default;

        private readonly Heart heart = new Heart();

        private void Start() => Setup();
        private void Update() => heart.Update();
        private void OnDisable() => heart.Dispose();
        private void OnApplicationQuit() => EventManager.RaiseEvent(Occasion.CLOSE_GAME);
        private void OnValidate() => CheckAssets();

        /// <summary>
        /// Chat I think this project is a little overengineerd but thats fun.
        /// </summary>
        private void Setup()
        {
            scenePrefabs.ForEach(x => Utils.SpawnPrefab(x));
            ServiceLocator<IAssetService>.Provide(assets);

            sceneSetup.Start();
            InputHandler inputHandler = InitializeInput(assets.GetBindingsConfig());
            ServiceLocator<IWorldService>.Provide(new GameWorld());

            // TEST:
            ServiceLocator<IWorldService>.Locate().Remove(gameObject);

            object[] behaviours = new object[]
            {
                inputHandler,
                new Interactor(),
                new EasyDebug(),
                new PlayerContext()
                // gotta add plants.
            };

            heart.Setup(behaviours);
        }

        private InputHandler InitializeInput(BindingsConfig bindingsConfig)
        {
            InputHandler inputHandler = new InputHandler(new DefaultBindingRules());
            List<Binding> bindings = bindingsConfig.GetBindings();
            bindings.ForEach(x => inputHandler.AddBinding(x));
            ServiceLocator<IInputService>.Provide(inputHandler);

            return inputHandler;
        }

        private void CheckAssets() 
        {
            string[] paths = 
            { 
                PLAYER,
                PLANT,
                PLAYER_SETTINGS,
                BINDINGS,
                INTERACT  
            };

            foreach (string path in paths)
            {
                if (Resources.Load(path) == null)
                    Debug.LogError($"{path} not found in resources! Please fix!");
            }
        }
    }
}