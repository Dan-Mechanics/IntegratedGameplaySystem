using System;
using UnityEngine;

namespace IntegratedGameplaySystem
{
    /// <summary>
    ///  you could say remove interface and then make bool for onetime purchse ?? or something ??
    /// </summary>
    public class OneTimePurchase : IPurchasable
    {
        // <summary>
        // Havent found use for this yet.
        // </summary>
        public bool HasBought => hasBought;
        
        public event Action OnBuy;
        public event Func<int, bool> OnCanBuy;

        private readonly UpgradeValuesInspector values;
        private readonly IWorldService world;
        private readonly GameObject button;

        /// <summary>
        /// Technically speaking this doesnt need to be here.
        /// </summary>
        private bool hasBought;

        public OneTimePurchase(Vector3 position, UpgradeValuesInspector values)
        {
            this.values = values;
            button = Utils.SpawnPrefab(values.buttonPrefab);
            button.transform.position = position;
            world = ServiceLocator<IWorldService>.Locate();
            world.Add(button, this);
        }

        public string GetHoverTitle()
        {
            /*if (hasBought)
                return string.Empty;*/

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