using System;
using UnityEngine;

namespace IntegratedGameplaySystem
{
    public class MultiblePurchase : IPurchasable
    {
        public event Action OnBuy;
        public event Func<int, bool> OnCanBuy;

        private readonly UpgradeProfile values;

        public MultiblePurchase(Vector3 position, UpgradeProfile values)
        {
            this.values = values;
            GameObject button = Utils.SpawnPrefab(values.buttonPrefab);
            button.transform.position = position;
            ServiceLocator<IWorldService>.Locate().Add(button, this);
        }

        public string GetHoverTitle()
        {
            if (!OnCanBuy.Invoke(values.cost))
                return "Can't afford yet!";

            return $"single buy {values.name} for ${values.cost}";
        }

        public void Interact()
        {
            if (!OnCanBuy.Invoke(values.cost))
                return;

            EventManager<int>.RaiseEvent(Occasion.LoseMoney, values.cost);
            OnBuy?.Invoke();
        }
    }
}