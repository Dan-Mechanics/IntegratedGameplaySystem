using System.Collections.Generic;
using UnityEngine;
using System;

namespace IntegratedGameplaySystem
{
    /// <summary>
    /// is there a way to make this use a pattern?
    /// Commonalityies:
    /// price
    /// interactable
    /// afford
    /// 
    /// You could say that the sprinkler lasts X amount of secodns right.
    /// </summary>
    [Serializable]
    public class UpgradeValues 
    {
        public string name;
        public int cost;
        public GameObject buttonPrefab;
    }
    
    //[System.Serializable]
    public class PermaUpgrade : IInteractable, IHoverable
    {
        public event Action OnBuy;
        public event Func<int, bool> OnCanAfford;

        public UpgradeValues values;

        private IWorldService world;
        private GameObject button;
        private bool hasBought;

        public void Setup(UpgradeValues values, IWorldService world, Vector3 offset)
        {
            this.world = world;
            this.values = values;

            button = Utils.SpawnPrefab(values.buttonPrefab);
            button.transform.position += offset;

            world.Add(button, this);
        }

        public bool GetHasBought() => hasBought;

        public string GetHoverTitle()
        {
            if (hasBought)
                return string.Empty;

            if (!OnCanAfford.Invoke(values.cost))
                return "Can't afford yet!";

            return $"{values.name} upgrade for ${values.cost}";
        }

        //public bool GetCanAfford() => OnCanAfford.Invoke(cost);

        public void Interact()
        {
            if (hasBought)
                return;

            if (!OnCanAfford.Invoke(values.cost))
                return;

            EventManager<int>.RaiseEvent(Occasion.LoseMoney, values.cost);
            hasBought = true;
            OnBuy?.Invoke();
            world.Remove(button);
        }
    }

   /* public class ConsumableUpgrade : IInteractable, IHoverable
    {
        public event Action OnBuy;
        public event Func<int, bool> OnCanAfford;

        private readonly string name;
        private readonly int cost;

        public ConsumableUpgrade(string name, int cost, GameObject go, IWorldService world)
        {
            this.name = name;
            this.cost = cost;
            world.Add(go, this);
        }

        public string GetHoverTitle()
        {
            if (!OnCanAfford.Invoke(cost))
                return "Can't afford yet!";

            return $"buy {name} for ${cost}";
        }

        public void Interact()
        {
            if (!OnCanAfford.Invoke(cost))
                return;

            EventManager<int>.RaiseEvent(Occasion.LoseMoney, cost);
            OnBuy?.Invoke();
        }
    }*/

}