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

            List<PlantFlyweight> flyweights = assetService.GetAllAssetsOfType<PlantFlyweight>();
            //IPlantSpawner spawner = new Dispersal() { dispersal = 20, plantCount = 30 };
            //IPlantSpawner spawner = new Plot(assetService.GetAssetByType<PlotSettings>(), );
            for (int i = 0; i < flyweights.Count; i++)
            {
                IPlantSpawner spawner = new Plot(assetService.GetAssetByType<PlotSettings>(), flyweights[i], new Vector3(i * 5f + 0.5f, 0f, 0.5f));
                spawner.Spawn(components);
            }

            // factory for this ??
            var tickClock = new Clock(assetService.GetAssetByType<ClockSettings>().interval);
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