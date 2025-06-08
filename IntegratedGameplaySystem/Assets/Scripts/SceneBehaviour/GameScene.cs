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
        private UpgradeSettings upgradeSettings;

        /// <summary>
        /// Builder for this ??
        /// </summary>
        public override List<object> GetSceneComponents()
        {            
            IAssetService assetService = ServiceLocator<IAssetService>.Locate();
            List<object> components = base.GetSceneComponents();

            var fps = new FirstPersonPlayer(new KeyboardSource(ServiceLocator<IInputService>.Locate()));
            components.Add(fps);
            
            var hand = new Hand();
            money = new MoneyCentral(hand);
            List<PlantFlyweight> plants = assetService.GetAllAssetsOfType<PlantFlyweight>();
            upgradeSettings = assetService.GetAssetByType<UpgradeSettings>();

            // Note: youcould add theup grade settings to the shit or not.
            

            var rainPool = new ObjectPool<PoolableParticle>();
            rainPool.AllocateNew += AllocateNewRain;
            ServiceLocator<IPoolService<PoolableParticle>>.Provide(rainPool);

            var plot = new Plot(assetService.GetAssetByType<PlotSettings>());
            var plantSpawner = new PlantSpawner(plot);

            upgradeSettings.area.Setup(plot.GetPlantCount());

            for (int i = 0; i < plants.Count; i++)
            {
                plot.SetPlotIndex(i);
                plantSpawner.SetPlant(plants[i]);

                plantSpawner.SpawnPlants(components);
                
                Vector3 plotUpgradePosition = plot.GetCenter();

                IUpgradeBehaviour sprinkler = new Sprinkler(new UpgradeCommonality(plotUpgradePosition, upgradeSettings.sprinkler), upgradeSettings);

                plotUpgradePosition += Vector3.up * upgradeSettings.grenadeHeight;
                IUpgradeBehaviour grenade = new Grenade(new UpgradeCommonality(plotUpgradePosition, upgradeSettings.grenade), upgradeSettings);

                /*components.Add(sprinkler);
                components.Add(grenade);*/
                upgrades.Add(sprinkler);
                upgrades.Add(grenade);
            }

            var shoes = new RunningShoes(new UpgradeCommonality(Vector3.zero, upgradeSettings.runningShoes), upgradeSettings, fps.Movement);
            var bigHands = new BigHands(new UpgradeCommonality(Vector3.up, upgradeSettings.bigHands), upgradeSettings, hand);

            upgrades.Add(shoes);
            upgrades.Add(bigHands);

            // idk if this works but whatever.
            upgrades.ForEach(x => components.Add(x));

            // factory for this ??
            var tickClock = new Clock(assetService.GetAssetByType<ClockSettings>().interval);
            var interactor = new Interactor();

            var score = new Score();
            ServiceLocator<IScoreService>.Provide(score);


            var display = new FarmingFrenzyDisplay(interactor, money, score, hand);
            components.Add(display);

            components.Add(score);
            components.Add(tickClock);
            components.Add(money);            
            components.Add(interactor);
            components.Add(hand);

            upgrades.ForEach(x => x.Upgrade.OnCanBuy += money.CanAfford);

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

        /// <summary>
        /// ?/ where do the assets belong at tho??
        /// </summary>
        /// <returns></returns>
        private PoolableParticle AllocateNewRain()
        {
            GameObject rain = Utils.SpawnPrefab(upgradeSettings.sprinklerEffect);
            return new PoolableParticle(rain.GetComponent<ParticleSystem>());
        }
    }
}