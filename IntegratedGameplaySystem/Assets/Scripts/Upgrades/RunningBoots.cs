using System;
using UnityEngine;

namespace IntegratedGameplaySystem
{
    public class RunningBoots : IGradeUp
    {
        public IPurchasable Purchasable => purchase;
        
        private readonly OneTimePurchase purchase;
        private readonly ForcesMovement movement;

        public RunningBoots(OneTimePurchase purchase, ForcesMovement movement)
        {
            this.purchase = purchase;
            this.movement = movement;
        }


        public void Dispose()
        {
            purchase.OnBuy -= Purchase_OnBuy;
        }

        public void Start()
        {
            purchase.OnBuy += Purchase_OnBuy;
        }

        private void Purchase_OnBuy()
        {
            // make movement fast here.
        }
    }
}