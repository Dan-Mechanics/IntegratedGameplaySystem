using System;
using UnityEngine;

namespace IntegratedGameplaySystem
{
    public class Grenade : IUpgradeBehaviour
    {
        private readonly MultiblePurchase purchase;
        private readonly Collider[] colliders;
        private readonly IWorldService world;
        private readonly Vector3 pos;
        private readonly UpgradeSettings settings;

        public Upgrade Upgrade { get; set; }

        public Grenade(MultiblePurchase purchase, int expectedColliders, Vector3 pos, UpgradeSettings settings)
        {
            this.purchase = purchase;
            this.pos = pos;
            this.settings = settings;
            colliders = new Collider[expectedColliders];
            world = ServiceLocator<IWorldService>.Locate();
        }

        public void Start() 
        {
            purchase.OnBuy += FarmAll;
        }

        public void Dispose()
        {
            purchase.OnBuy -= FarmAll;
        }

        private void FarmAll() 
        {
            Transform effect = Utils.SpawnPrefab(settings.grenadeEffect).transform;
            effect.position = pos;

            int length = Physics.OverlapSphereNonAlloc(pos, settings.range, colliders, settings.mask, QueryTriggerInteraction.Ignore);
            for (int i = 0; i < length; i++)
            {
                // you could add a ? here but I think we can assume it works.
                world.GetComponent<IHarvestable>(colliders[i].transform).Harvest();
            }
        }
    }
}