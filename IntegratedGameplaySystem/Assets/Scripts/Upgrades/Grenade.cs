using System;
using UnityEngine;

namespace IntegratedGameplaySystem
{
    public class Grenade : IUpgradeBehaviour
    {
        public UpgradeCommonality Upgrade { get; set; }
        private readonly IWorldService world;
        private readonly GrenadeSettings settings;

        public Grenade(UpgradeCommonality Upgrade, GrenadeSettings settings)
        {
            this.settings = settings;
            this.Upgrade = Upgrade;
            world = ServiceLocator<IWorldService>.Locate();
        }

        public void Start() 
        {
            Upgrade.OnBuy += HarvestAll;
        }

        public void Dispose()
        {
            Upgrade.OnBuy -= HarvestAll;
        }

        private void HarvestAll() 
        {
            Transform effect = Utils.SpawnPrefab(settings.grenadeEffect).transform;
            effect.position = Upgrade.Position;

            for (int i = 0; i < settings.area.GetCollidersHitSphere(Upgrade.Position); i++)
            {
                // you could add a ? here but I think we can assume it works.
                world.GetComponent<IHarvestable>(settings.area.colliders[i].transform).Harvest();
            }
        }
    }
}