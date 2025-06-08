using System;
using UnityEngine;

namespace IntegratedGameplaySystem
{
    public class OneTimePurchase : IPurchasable
    {
        // <summary>
        // Havent found use for this yet.
        // </summary>
        //public bool HasBought => hasBought;
        
        public event Action OnBuy;
        public event Func<int, bool> OnCanBuy;

        private readonly UpgradeValuesInspector values;
        private readonly IWorldService world;
        private readonly GameObject button;

        private bool hasBought;

        public OneTimePurchase(Vector3 position, UpgradeValuesInspector values, GameObject buttonPrefab)
        {
            this.values = values;
            button = Utils.SpawnPrefab(buttonPrefab);
            button.transform.position = position;
            world = ServiceLocator<IWorldService>.Locate();
            world.Add(button, this);
        }

        public string GetHoverTitle()
        {
            if (hasBought)
                return string.Empty;

            if (!OnCanBuy.Invoke(values.cost))
                return "Can't afford yet!";

            return $"{values.name} upgrade for ${values.cost}";
        }

        public void Interact()
        {
            // tecnucally speakign this aint required but ok.
            if (hasBought)
                return;
            
            if (!OnCanBuy.Invoke(values.cost))
                return;

            hasBought = true;
            world.Remove(button);
            EventManager<int>.RaiseEvent(Occasion.LoseMoney, values.cost);
            OnBuy?.Invoke();
        }
    }
}