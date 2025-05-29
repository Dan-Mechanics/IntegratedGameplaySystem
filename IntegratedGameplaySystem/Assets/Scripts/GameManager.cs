using System.Collections.Generic;
using UnityEngine;

namespace IntegratedGameplaySystem
{
    /// <summary>
    /// SINGLE MONOBEHAVIOUR HERE !!
    /// </summary>
    public class GameManager : MonoBehaviour 
    {
        [SerializeField] private SceneSetup sceneSetup = default;
        [SerializeField] private List<GameObject> scenePrefabs = default;
        [SerializeField] private Assets assets = default;

        private readonly Heart heart = new Heart();

        private void Start() => Setup();
        private void Update() => heart.Update();
        private void OnDisable() => heart.Dispose();
        private void OnApplicationQuit() => EventManager.RaiseEvent(Occasion.CLOSE_GAME);

        private void Setup()
        {
            scenePrefabs.ForEach(x => Utils.SpawnPrefab(x));
            ServiceLocator<IAssetService>.Provide(assets);

            sceneSetup.Start();
            InputHandler inputHandler = InitializeInput(assets.GetBindingsConfig());
            ServiceLocator<IWorldService>.Provide(new GameWorld());

            // TEST:
            ServiceLocator<IWorldService>.Locate().Remove(gameObject);

            object[] components = new object[]
            {
                inputHandler,
                new Interactor(),
                new EasyDebug(),
                new PlayerContext()
                // gotta add plants.
            };

            heart.Setup(components);
        }

        private InputHandler InitializeInput(BindingsConfig bindingsConfig)
        {
            InputHandler inputHandler = new InputHandler(new DefaultBindingRules());
            List<Binding> bindings = bindingsConfig.GetBindings();
            bindings.ForEach(x => inputHandler.AddBinding(x));
            ServiceLocator<IInputService>.Provide(inputHandler);

            return inputHandler;
        }
    }
}