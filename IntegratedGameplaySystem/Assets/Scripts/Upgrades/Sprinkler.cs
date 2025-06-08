using System;
using UnityEngine;

namespace IntegratedGameplaySystem
{
    public class Sprinkler : IGradeUp
    {
        private readonly OneTimePurchase purchase;
        private readonly Collider[] colliders;
        private readonly IWorldService world;
        private readonly float range;
        private readonly Vector3 pos;
        private readonly LayerMask mask;

        private bool isTicking;

        public IPurchasable Purchasable => purchase;

        public Sprinkler(OneTimePurchase purchase, int expectedColliders, float range, Vector3 pos, LayerMask mask)
        {
            this.purchase = purchase;
            this.range = range;
            this.pos = pos;
            this.mask = mask;
            colliders = new Collider[expectedColliders];
            world = ServiceLocator<IWorldService>.Locate();
        }

        public void Start() 
        {
            purchase.OnBuy += SubscribeToTicks;
        }

        private void SubscribeToTicks()
        {
            EventManager.AddListener(Occasion.LateTick, LateTick);
            isTicking = true;
        }

        public void Dispose()
        {
            // is this needed ?
            if (isTicking)
                EventManager.RemoveListener(Occasion.LateTick, LateTick);

            purchase.OnBuy -= SubscribeToTicks;
        }

        /// <summary>
        /// Mihgt nee dto add late tick.
        /// </summary>
        private void LateTick() 
        {
            int length = Physics.OverlapSphereNonAlloc(pos, range, colliders, mask, QueryTriggerInteraction.Ignore);
            
            for (int i = 0; i < length; i++)
            {
                // you could add a ? here but I think we can assume it works.
                world.GetComponent<IWaterable>(colliders[i].transform).Water();
            }
        }
    }
}