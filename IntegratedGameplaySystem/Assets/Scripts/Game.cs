using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace IntegratedGameplaySystem
{
    [CreateAssetMenu(menuName = "ScriptableObjects/" + nameof(Game), fileName = "New " + nameof(Game))]
    public class Game : ScriptableObject, IScene, IDisposable
    {
        public string nextScene;
        
        public void Dispose()
        {
            EventManager.RemoveListener(Occasion.GAME_OVER, NextScene);
        }

        /// <summary>
        /// Cosndier moving this somewhere else.
        /// Litterally lvoe coding silly things like this:
        /// And suddenly im me again.
        /// 
        /// MASSIVE COUPLING EMERGY HERE !!
        /// </summary>
        public List<object> GetGameBehaviours()
        {
            ServiceLocator<IWorldService>.Provide(new GameWorld());
            ServiceLocator<IInputService>.Provide(new InputHandler(new ChillBindingRules(), new ConfigTextFile()));

            List<object> behaviours = new()
            {
                ServiceLocator<IInputService>.Locate(),
                new Player(new KeyboardSource()),
                new TestingFeatures(),
                new TickClock()
            };

            var interactor = new Interactor();
            behaviours.Add(interactor);

            var wallet = new Wallet();
            behaviours.Add(wallet);

            var plantSpecies = ServiceLocator<IAssetService>.Locate().GetCollectionType<PlantBlueprint>();

            //IPlantSpawner spawner = new Dispersal() { dispersal = 20, plantCount = 30 };
            IPlantSpawner spawner = new Plot(5, 1f);
            for (int i = 0; i < plantSpecies.Count; i++)
            {
                spawner.Spawn(behaviours, plantSpecies[i], new Vector3(i * 5f, 0f, 0f));
            }

            var display = new Display(interactor, wallet);
            behaviours.Add(display);

            behaviours.Add(this);

            EventManager.AddListener(Occasion.GAME_OVER, NextScene);

            return behaviours;
        }

        private void NextScene()
        {
            SceneManager.LoadScene(nextScene);
        }
    }
}