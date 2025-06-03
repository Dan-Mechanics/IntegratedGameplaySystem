using System.Collections.Generic;
using UnityEngine;

namespace IntegratedGameplaySystem
{
    [CreateAssetMenu(menuName = "ScriptableObjects/" + nameof(Game), fileName = "New " + nameof(Game))]
    public class Game : SceneBehaviour
    {
        public override List<object> GetSceneComponents()
        {
            IAssetService assetService = ServiceLocator<IAssetService>.Locate();
            List<object> components = base.GetSceneComponents();

            //ServiceLocator<IWorldService>.Provide(new GameWorld());
            components.Add(new Player(new KeyboardSource(ServiceLocator<IInputService>.Locate())));

            var tickClock = new Clock(assetService.GetAssetWithType<ClockSettings>().interval);
            components.Add(tickClock);

            ServiceLocator<IScoreService>.Provide(tickClock);

            var interactor = new Interactor();
            components.Add(interactor);

            var wallet = new MoneyCentral();
            components.Add(wallet);

            List<PlantBlueprint> plantBlueprints = assetService.GetAssetsOfType<PlantBlueprint>();

            //IPlantSpawner spawner = new Dispersal() { dispersal = 20, plantCount = 30 };
            IPlantSpawner spawner = new Plot(5, 1f);
            for (int i = 0; i < plantBlueprints.Count; i++)
            {
                spawner.Spawn(components, plantBlueprints[i], new Vector3(i * 5f + 0.5f, 0f, 0.5f));
            }

            var display = new Display(interactor, wallet, tickClock);
            components.Add(display);

            return components;
        }
    }
}