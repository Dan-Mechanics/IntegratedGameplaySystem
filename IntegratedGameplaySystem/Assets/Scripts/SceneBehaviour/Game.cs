using System.Collections.Generic;
using UnityEngine;

namespace IntegratedGameplaySystem
{
    /// <summary>
    /// Currently somewhat serves as a context class.
    /// </summary>
    [CreateAssetMenu(menuName = "ScriptableObjects/" + nameof(Game), fileName = "New " + nameof(Game))]
    public class Game : SceneBehaviour
    {
        private Plot[] plots;
        private MoneyCentral money;

        public override List<object> GetSceneComponents()
        {
            IAssetService assetService = ServiceLocator<IAssetService>.Locate();
            List<object> components = base.GetSceneComponents();

            components.Add(new FirstPersonPlayer(new KeyboardSource(ServiceLocator<IInputService>.Locate())));
            
            var hand = new Hand();
            money = new MoneyCentral(hand);
            
            List<PlantFlyweight> flyweights = assetService.GetAllAssetsOfType<PlantFlyweight>();
            plots = new Plot[flyweights.Count];

            for (int i = 0; i < flyweights.Count; i++)
            {
                plots[i] = new Plot(assetService.GetAssetByType<PlotSettings>(), flyweights[i], i);
                plots[i].CanAffordUpgrade += money.CanAfford;
                plots[i].SpawnPlants(components);
            }

            // factory for this ??
            var tickClock = new Clock(assetService.GetAssetByType<ClockSettings>().interval);
            var score = new Score();
            ServiceLocator<IScoreService>.Provide(score);

            
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

        public override void Dispose()
        {
            base.Dispose();

            for (int i = 0; i < plots.Length; i++)
            {
                plots[i].CanAffordUpgrade -= money.CanAfford;
            }

            money = null;
            plots = null;
        }
    }
}