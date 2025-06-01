using System.Collections.Generic;
using UnityEngine;

namespace IntegratedGameplaySystem
{
    [CreateAssetMenu(menuName = "ScriptableObjects/" + nameof(Game), fileName = "New " + nameof(Game))]
    public class Game : Scene
    {
        public override List<object> GetGame()
        {
            List<object> behaviours = base.GetGame();
            
            ServiceLocator<IWorldService>.Provide(new GameWorld());
            //ServiceLocator<IInputService>.Provide(new InputHandler(new ChillBindingRules(), new ConfigTextFile()));

            behaviours.Add(ServiceLocator<IInputService>.Locate());
            behaviours.Add(new Player(new KeyboardSource()));

            ServiceLocator<IScoreService>.Provide(null);

            var tickClock = new Clock();
            Debug.Log(tickClock);
            behaviours.Add(tickClock);

            ServiceLocator<IScoreService>.Provide(tickClock);

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

            var display = new Display(interactor, wallet, tickClock);
            behaviours.Add(display);

            //behaviours.AddRange(base.GetGame());

            return behaviours;
        }
    }
}