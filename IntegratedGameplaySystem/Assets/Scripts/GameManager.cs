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
        /// </summary>
        private void Setup()
        {
            sceneSetup.Start();
            scenePrefabs.ForEach(x => Utils.SpawnPrefab(x));

            ServiceLocator<IAssetService>.Provide(assets);
            ServiceLocator<IWorldService>.Provide(new GameWorld());

            IInputService inputService = new InputHandler(new DefaultBindingRules(), new ConfigTextFile());
            //IInputService inputService = new InputHandler(new DefaultBindingRules(), assets.GetByType<BindingsConfig>());
            ServiceLocator<IInputService>.Provide(inputService);

            List<object> behaviours = new List<object>
            {
                inputService,
                new PlayerMovement(new KeyboardSource()),
                new Interactor(),
                new EasyDebug(),
                new TickClock()
            };

            List<PlantSpeciesProfile> profiles = assets.GetCollectionType<PlantSpeciesProfile>();
            for (int i = 0; i < profiles.Count; i++)
            {
                for (int j = 0; j < profiles[i].plantCount; j++)
                {
                    // !PERFORMANCE
                    behaviours.Add(new Plant(profiles[i]));
                }
            }

            heart.Setup(behaviours);
        }
    }
}