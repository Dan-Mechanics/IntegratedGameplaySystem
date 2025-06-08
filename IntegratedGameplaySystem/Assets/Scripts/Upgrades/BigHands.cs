using System;
using UnityEngine;

namespace IntegratedGameplaySystem
{
    public class BigHands : IUpgradeBehaviour
    {
        private readonly OneTimePurchase purchase;
        private readonly Hand hand;

        public BigHands(OneTimePurchase purchase, Hand hand)
        {
            this.purchase = purchase;
            this.hand = hand;
        }

        public IPurchasable Purchasable => purchase;

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
            // make more ivnentory space here.
        }
    }
}