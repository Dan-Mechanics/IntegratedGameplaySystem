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
            /*ServiceLocator<IWorldService>.Provide(new GameWorld());
            ServiceLocator<IInputService>.Provide(new InputHandler(new ChillBindingRules(), new ConfigTextFile()));*/

            if (scene is not IScene foundScene)
            {
                Debug.LogError("Please assign a valid scene.");
                return;
            }

            //IGame game = new FarmingFrenzy();
            heart.Setup(foundScene.GetGameBehaviours());
        }
    }
}