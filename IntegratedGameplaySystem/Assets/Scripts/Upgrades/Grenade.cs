using System;
using UnityEngine;

namespace IntegratedGameplaySystem
{
    public class Grenade : IStartable, IDisposable
    {
        private readonly MultiblePurchase purchase;
        private readonly Collider[] colliders;
        private readonly IWorldService world;
        private readonly float range;
        private readonly Vector3 pos;
        private readonly LayerMask mask;
        private readonly GameObject effectPrefab;

        public Grenade(MultiblePurchase purchase, int expectedColliders, float range, Vector3 pos, LayerMask mask, GameObject effectPrefab)
        {
            this.purchase = purchase;
            this.effectPrefab = effectPrefab;
            this.range = range;
            this.pos = pos;
            this.mask = mask;
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
            int length = Physics.OverlapSphereNonAlloc(pos, range, colliders, mask, QueryTriggerInteraction.Ignore);

            Transform effect = Utils.SpawnPrefab(effectPrefab).transform;
            effect.position = pos;

            for (int i = 0; i < length; i++)
            {
                // you could add a ? here but I think we can assume it works.
                world.GetComponent<IHarvestable>(colliders[i].transform).Harvest();
            }
        }
    }
}