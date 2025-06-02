using System.Collections.Generic;
using UnityEngine;

namespace IntegratedGameplaySystem
{
    [CreateAssetMenu(menuName = "ScriptableObjects/" + nameof(Game), fileName = "New " + nameof(Game))]
    public class Game : ScriptableObject, IScene, IDisposable
    {
        public SceneHandler sceneHandler;

        public List<object> GetSceneBehaviours()
        {
            sceneHandler.Start();
            List<object> behaviours = new List<object>();

            ServiceLocator<IWorldService>.Provide(new GameWorld());
            behaviours.Add(new Player(new KeyboardSource(ServiceLocator<IInputService>.Locate())));

            var tickClock = new Clock();
            behaviours.Add(tickClock);

            ServiceLocator<IScoreService>.Provide(tickClock);

            var interactor = new Interactor();
            behaviours.Add(interactor);

            var wallet = new MoneyCentral();
            behaviours.Add(wallet);

            var plantSpecies = ServiceLocator<IAssetService>.Locate().GetCollectionType<PlantBlueprint>();

            //IPlantSpawner spawner = new Dispersal() { dispersal = 20, plantCount = 30 };
            IPlantSpawner spawner = new Plot(5, 1f);
            for (int i = 0; i < plantSpecies.Count; i++)
            {
                spawner.Spawn(behaviours, plantSpecies[i], new Vector3(i * 5f + 0.5f, 0f, 0.5f));
            }

            var display = new Display(interactor, wallet, tickClock);
            behaviours.Add(display);

            return behaviours;
        }

        public void Dispose()
        {
            sceneHandler.Dispose();
        }
    }
}