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
        public GameObject rainEffectPrefab;
        //public ObjectPool<PoolableParticle> rainPool;

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
            var money = new MoneyCentral(hand);
            List<PlantFlyweight> plants = assetService.GetAllAssetsOfType<PlantFlyweight>();
            UpgradeSettings upgrade = assetService.GetAssetByType<UpgradeSettings>();

            // Note: youcould add theup grade settings to the shit or not.
            IPlantPlacementStrategy strat = new Plot(assetService.GetAssetByType<PlotSettings>());

            var rainPool = new FastPool<PoolableParticle>(strat.GetPlantCount());
            rainPool.AllocateNew += AllocateNewRain;
            rainPool.Fill();
            ServiceLocator<IPoolService<PoolableParticle>>.Provide(rainPool);

            for (int i = 0; i < plants.Count; i++)
            {
                var plantHolder = new PlantHolder(upgrade, i, plants[i], money, strat);
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

            return components;
        }

        public override void Dispose()
        {
            base.Dispose();

            var pool = ServiceLocator<IPoolService<PoolableParticle>>.Locate();
            pool.AllocateNew -= AllocateNewRain;
            pool.Flush();
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