using System;
using UnityEngine;

namespace IntegratedGameplaySystem
{
    /// <summary>
    /// is there a way to make this use a pattern?
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
        public event Action OnBuy;
        public event Func<int, bool> OnCanAfford;
        void Setup(IWorldService world, Vector3 offset, UpgradeValues values);
    }

    public class PermanentUpgrade : IUpgradable
    {
        public event Action OnBuy;
        public event Func<int, bool> OnCanAfford;
        
        private IWorldService world;
        private bool hasBought;
        private UpgradeValues values;
        private GameObject button;


        public void Setup(IWorldService world, Vector3 offset, UpgradeValues values)
        {
            this.values = values;
            this.world = world;

            button = Utils.SpawnPrefab(values.buttonPrefab);
            button.transform.position += offset;

            world.Add(button, this);
        }

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

    public class TemporaryUpgrade : IUpgradable
    {
        public event Action OnBuy;
        public event Func<int, bool> OnCanAfford;

        private UpgradeValues values;
        private GameObject button;

        public void Setup(IWorldService world, Vector3 offset, UpgradeValues values)
        {
            this.values = values;
            button = Utils.SpawnPrefab(values.buttonPrefab);
            button.transform.position += offset;

            world.Add(button, this);
        }

        public string GetHoverTitle()
        {
            if (!OnCanAfford.Invoke(values.cost))
                return "Can't afford yet!";

            return $"{values.name} for ${values.cost}";
        }

        public void Interact()
        {
            if (!OnCanAfford.Invoke(values.cost))
                return;

            EventManager<int>.RaiseEvent(Occasion.LoseMoney, values.cost);
            OnBuy?.Invoke();
        }
    }
}