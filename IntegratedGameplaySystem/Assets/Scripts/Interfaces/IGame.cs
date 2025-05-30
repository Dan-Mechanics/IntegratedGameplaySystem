using System.Collections.Generic;
using UnityEngine;

namespace IntegratedGameplaySystem
{
    public interface IGame: IDisposable
    {
        List<object> GetGameBehaviours();
    }

    public class FarmingFrenzy : IGame 
    {
        private Plant[] plants;
        
        /// <summary>
        /// Cosndier moving this somewhere else.
        /// Litterally lvoe coding silly things like this:
        /// And suddenly im me again.
        /// 
        /// MASSIVE COUPLING EMERGY HERE !!
        /// </summary>
        public List<object> GetGameBehaviours()
        {
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

            List<Plant> plants = new List<Plant>();
            //IPlantSpawner spawner = new Dispersal() { dispersal = 20, plantCount = 30 };
            IPlantSpawner spawner = new Plot(5, 1f);
            for (int i = 0; i < plantSpecies.Count; i++)
            {
                spawner.Spawn(behaviours, plantSpecies[i], new Vector3(i * 5f, 0f, 0f), plants);
            }

            this.plants = plants.ToArray();

            var display = new Display(interactor, wallet);
            behaviours.Add(display);

            EventManager.AddListener(Occasion.TICK, Tick);
            EventManager.AddListener(Occasion.GAME_OVER, Application.Quit);

            behaviours.Add(this);
            return behaviours;
        }

        private void Tick() 
        {
            for (int i = 0; i < plants.Length; i++)
            {
                plants[i].Tick();
            }
        }

        public void Dispose() 
        {
            EventManager.RemoveListener(Occasion.TICK, Tick);
            EventManager.RemoveListener(Occasion.GAME_OVER, Application.Quit);
        }
    }
}