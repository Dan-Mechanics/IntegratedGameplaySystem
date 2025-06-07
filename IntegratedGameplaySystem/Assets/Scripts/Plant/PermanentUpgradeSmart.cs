using System;
using UnityEngine;

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

    public interface IUpgradable : IInteractable, IHoverable 
    {
        Action OnBuy { get; set; }
        Func<int, bool> OnCanAfford { get; set; }
        //void Setup(IWorldService world, Vector3 offset, UpgradeValues values);
    }

    public class PermanentUpgradeSmart : IUpgradable
    {
        private IWorldService world;
        private bool hasBought;
        private UpgradeValues values;
        private GameObject button;

        public event Action OnBuy;
        public event Func<int, bool> OnCanAfford;

        public void Setup(IWorldService world, Vector3 offset, UpgradeValues values)
        {
            this.values = values;
            this.world = world;

            button = Utils.SpawnPrefab(values.buttonPrefab);
            button.transform.position += offset;

            world.Add(button, this);
        }

        //public bool GetHasBought() => hasBought;

        public string GetHoverTitle()
        {
            if (hasBought)
                return string.Empty;

            if (!OnCanAfford.Invoke(values.cost))
                return "Can't afford yet!";

            return $"{values.name} upgrade for ${values.cost}";
        }

        public void Interact()
        {
            if (hasBought)
                return;

            if (!OnCanAfford.Invoke(values.cost))
                return;

            OnBuy?.Invoke();
            EventManager<int>.RaiseEvent(Occasion.LoseMoney, values.cost);
            hasBought = true;
            world.Remove(button);
        }
    }

    public class PermanentUpgradeDumb : TemporaryUpgrade
    {
        //private IWorldService world;
        private bool hasBought;

        public override string GetHoverTitle()
        {
            if (hasBought)
                return string.Empty;

            if (!OnCanAfford.Invoke(values.cost))
                return "Can't afford yet!";

            return $"{values.name} upgrade for ${values.cost}";
        }

        public override void Interact()
        {
            if (hasBought)
                return;

            if (!OnCanAfford.Invoke(values.cost))
                return;

            OnBuy?.Invoke();
            EventManager<int>.RaiseEvent(Occasion.LoseMoney, values.cost);
            hasBought = true;
            world.Remove(button);
        }
    }

    public class TemporaryUpgrade : IUpgradable
    {

        protected readonly UpgradeValues values;
        protected readonly IWorldService world;
        protected readonly GameObject button;

        public Action OnBuy;
        public Func<int, bool> OnCanAfford;

        public TemporaryUpgrade(IWorldService world, Vector3 offset, UpgradeValues values)
        {
            this.values = values;
            button = Utils.SpawnPrefab(values.buttonPrefab);
            button.transform.position += offset;

            this.world = world;
            world.Add(button, this);
        }

        public virtual string GetHoverTitle()
        {
            if (!OnCanAfford.Invoke(values.cost))
                return "Can't afford yet!";

            return $"{values.name} for ${values.cost}";
        }

        public virtual void Interact()
        {
            if (!OnCanAfford.Invoke(values.cost))
                return;

            EventManager<int>.RaiseEvent(Occasion.LoseMoney, values.cost);
            OnBuy?.Invoke();
        }
    }
}