using System.Collections.Generic;
using UnityEngine;

namespace IntegratedGameplaySystem
{
    /// <summary>
    /// Currently somewhat serves as a context class.
    /// </summary>
    [CreateAssetMenu(menuName = "ScriptableObjects/" + nameof(GameScene), fileName = "New " + nameof(GameScene))]
    public class GameScene : SceneBehaviour
    {
        private readonly List<IUpgradeBehaviour> upgrades = new();

        private MoneyCentral money;
        private GameObject rainEffect;

        /// <summary>
        /// Builder for this ??
        /// </summary>
        public override List<object> GetSceneComponents()
        {            
            IAssetService assets = ServiceLocator<IAssetService>.Locate();
            List<object> components = base.GetSceneComponents();

            // ======================

            var player = new FirstPersonPlayer(new KeyboardSource(ServiceLocator<IInputService>.Locate()));
            components.Add(player);
            
            var hand = new Hand(assets.GetAssetByType<HandSettings>());
            money = new MoneyCentral(hand);
            List<PlantFlyweight> plantsFlyweights = assets.GetAllAssetsOfType<PlantFlyweight>();

            // ======================

            var rainPool = new ObjectPool<PoolableParticle>();
            rainPool.AllocateNew += AllocateNewRain;
            ServiceLocator<IPoolService<PoolableParticle>>.Provide(rainPool);

            // ======================

            var plot = new Plot(assets.GetAssetByType<PlotSettings>());
            var plantSpawner = new PlantSpawner(plot);

            // ======================

            var sprinklerSettings = assets.GetAssetByType<SprinklerSettings>();
            sprinklerSettings.area.Setup(plot.GetPlantCount());

            rainEffect = sprinklerSettings.rainEffect;

            var grenadeSettings = assets.GetAssetByType<GrenadeSettings>();
            grenadeSettings.area.Setup(plot.GetPlantCount());

            // ======================

            for (int i = 0; i < plantsFlyweights.Count; i++)
            {
                plot.SetPlotIndex(i);
                plantSpawner.SetPlant(plantsFlyweights[i]);

                plantSpawner.SpawnPlants(components);
                
                Vector3 plotCenter = plot.GetCenter();

                IUpgradeBehaviour sprinkler = new Sprinkler(new UpgradeCommonality(plotCenter, sprinklerSettings), sprinklerSettings);
                IUpgradeBehaviour grenade = new Grenade(new UpgradeCommonality(plotCenter, grenadeSettings), grenadeSettings);

                upgrades.Add(sprinkler);
                upgrades.Add(grenade);
            }

            var shoesSettings = assets.GetAssetByType<RunningShoesSettings>();
            var bagSettings = assets.GetAssetByType<BagSettings>();

            IUpgradeBehaviour shoes = new RunningShoes(new UpgradeCommonality(Vector3.zero, shoesSettings), shoesSettings, player.Movement);
            IUpgradeBehaviour bag = new Bag(new UpgradeCommonality(Vector3.zero, bagSettings), bagSettings, hand);

            upgrades.Add(shoes);
            upgrades.Add(bag);

            // idk if this works but whatever.
            upgrades.ForEach(x => components.Add(x));
            upgrades.ForEach(x => x.Upgrade.OnCanBuy += money.CanAfford);

            // ======================

            // factory for this ??
            var tickClock = new Clock(assets.GetAssetByType<ClockSettings>().interval);
            var interactor = new Interactor();

            var score = new Score();
            ServiceLocator<IScoreService>.Provide(score);

            // ======================

            var display = new FarmingFrenzyDisplay(interactor, money, score, hand);
            components.Add(display);

            components.Add(score);
            components.Add(tickClock);
            components.Add(money);            
            components.Add(interactor);
            components.Add(hand);

            // ======================

            return components;
        }

        public override void Dispose()
        {
            base.Dispose();

            var pool = ServiceLocator<IPoolService<PoolableParticle>>.Locate();
            pool.AllocateNew -= AllocateNewRain;
            pool.Flush();

            upgrades.ForEach(x => x.Upgrade.OnCanBuy -= money.CanAfford);
        }

        private PoolableParticle AllocateNewRain()
        {
            return new PoolableParticle(
                Utils.SpawnPrefab(rainEffect).GetComponent<ParticleSystem>());
        }
    }
}