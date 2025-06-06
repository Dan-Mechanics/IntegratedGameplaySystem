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
            components.Add(new FirstPersonPlayer(new KeyboardSource(ServiceLocator<IInputService>.Locate())));

            List<PlantFlyweight> plantBlueprints = assetService.GetAssetsOfType<PlantFlyweight>();
            //IPlantSpawner spawner = new Dispersal() { dispersal = 20, plantCount = 30 };
            IPlantSpawner spawner = new Plot(5, 1f);
            for (int i = 0; i < plantBlueprints.Count; i++)
            {
                spawner.Spawn(components, plantBlueprints[i], new Vector3(i * 5f + 0.5f, 0f, 0.5f));
            }

            // factory for this ??
            var tickClock = new Clock(assetService.GetAssetWithType<ClockSettings>().interval);
            var score = new Score();
            ServiceLocator<IScoreService>.Provide(score);

            var hand = new Hand();
            var money = new MoneyCentral(hand);
            var interactor = new Interactor();

            Display display = new Display(interactor, money, score, hand);
            components.Add(display);

            components.Add(score);
            components.Add(tickClock);
            components.Add(money);            
            components.Add(interactor);
            components.Add(hand);

            return components;
        }
    }
}