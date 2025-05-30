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
        /// Consider moving this setup to an inhertied thng.
        /// </summary>
        private void Setup()
        {
            sceneSetup.Start();
            scenePrefabs.ForEach(x => Utils.SpawnPrefab(x));

            ServiceLocator<IAssetService>.Provide(assets);
            ServiceLocator<IWorldService>.Provide(new GameWorld());
            ServiceLocator<IInputService>.Provide(new InputHandler(new ChillBindingRules(), new ConfigTextFile()));

            heart.Setup(GetGameBehaviours());
        }

        /// <summary>
        /// Cosndier moving this somewhere else.
        /// Litterally lvoe coding silly things like this:
        /// And suddenly im me again.
        /// 
        /// MASSIVE COUPLING EMERGY HERE !!
        /// </summary>
        private List<object> GetGameBehaviours() 
        {
            List<object> result = new List<object>
            {
                ServiceLocator<IInputService>.Locate(),
                new PlayerMovement(new KeyboardSource()),
                //new Interactor(),
                new TestingFeatures(),
                new TickClock()
            };

            Display display = new Display(assets.GetByAgreedName(Display.CANVAS_PREFAB_NAME));
            Interactor interactor = new Interactor();
            interactor.OnHoverChange += display.UpdateHoveringText;

            result.Add(interactor);
            result.Add(display);

            List<PlantSpeciesProfile> blueprints = assets.GetCollectionType<PlantSpeciesProfile>();
            Wallet wallet = new Wallet();

            for (int i = 0; i < blueprints.Count; i++)
            {
                for (int j = 0; j < blueprints[i].plantCount; j++)
                {
                    result.Add(new Plant(blueprints[i], assets.GetByAgreedName(Plant.PLANT_PREFAB_NAME),
                        assets.GetByAgreedName(Plant.RAIN_PREFAB_NAME)));
                }
            }

            wallet.OnMoneyChanged += display.UpdateMoneyText;
            result.Add(wallet);

            return result;
        }
    }
}