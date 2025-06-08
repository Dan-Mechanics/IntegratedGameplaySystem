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
        // make game assets SO or something.
        // move all of this to ugpradesettigns.
        public GameObject rainEffectPrefab;
        //public GameObject buttonPrefab;
        //public GameObject grenadeEffectPrefab;
        public float range;
        public LayerMask mask;
        //public ObjectPool<PoolableParticle> rainPool;

        private List<IGradeUp> gradeUps = new List<IGradeUp>();
        private MoneyCentral money;

        /// <summary>
        /// Builder for this /???
        /// </summary>
        /// <returns></returns>
        public override List<object> GetSceneComponents()
        {            
            IAssetService assetService = ServiceLocator<IAssetService>.Locate();
            List<object> components = base.GetSceneComponents();

            components.Add(new FirstPersonPlayer(new KeyboardSource(ServiceLocator<IInputService>.Locate())));
            
            var hand = new Hand();
            money = new MoneyCentral(hand);
            List<PlantFlyweight> plants = assetService.GetAllAssetsOfType<PlantFlyweight>();
            UpgradeSettings upgrade = assetService.GetAssetByType<UpgradeSettings>();

            // Note: youcould add theup grade settings to the shit or not.
            IPlantPlacementStrategy strat = new Plot(assetService.GetAssetByType<PlotSettings>());

            var rainPool = new ObjectPool<PoolableParticle>();
            rainPool.AllocateNew += AllocateNewRain;
            ServiceLocator<IPoolService<PoolableParticle>>.Provide(rainPool);

            for (int i = 0; i < plants.Count; i++)
            {
                var plantHolder = new PlantCollectionHandler(i, plants[i], strat);

                Vector3 pos = strat.GetPlotPos(i) + Vector3.up * 2;
                pos += Vector3.forward;

                IGradeUp sprinkler = new Sprinkler(new OneTimePurchase(pos, upgrade.sprinkler), strat.GetPlantCount(), range, pos, mask);

                pos += Vector3.forward * 2f;

                IGradeUp grenade = new Grenade(new MultiblePurchase(pos, upgrade.grenade), strat.GetPlantCount(), range, pos, mask, upgrade.grenadeEffect);

                components.Add(sprinkler);
                components.Add(grenade);
                gradeUps.Add(sprinkler);
                gradeUps.Add(grenade);
                plantHolder.SpawnPlants(components);
            }

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

            gradeUps.ForEach(x => x.Purchasable.OnCanBuy += money.CanAfford);

            return components;
        }

        public override void Dispose()
        {
            base.Dispose();

            var pool = ServiceLocator<IPoolService<PoolableParticle>>.Locate();
            pool.AllocateNew -= AllocateNewRain;
            pool.Flush();

            gradeUps.ForEach(x => x.Purchasable.OnCanBuy -= money.CanAfford);
        }

        /// <summary>
        /// ?/ where do the assets belong at tho??
        /// </summary>
        /// <returns></returns>
        private PoolableParticle AllocateNewRain()
        {
            GameObject rain = Utils.SpawnPrefab(rainEffectPrefab);
            return new PoolableParticle(rain.GetComponent<ParticleSystem>());
        }
    }
}