using System;
using UnityEngine;

namespace IntegratedGameplaySystem
{
    /// <summary>
    /// is there a way to make this use a pattern?
    /// You could say that the sprinkler lasts X amount of secodns right. --> i like that it lasts.
    /// </summary>
    [Serializable]
    public class UpgradeValuesInspector
    {
        public string name;
        public int cost;
        public GameObject buttonPrefab;
    }

    public interface IUpgradable : IInteractable, IHoverable 
    {
        public event Func<int, bool> OnCanBuy;
        public event Action OnBuy;
        void SetValues(UpgradeValuesInspector values);
        void Setup(Vector3 offset, IWorldService world);
    }

    public class PermanentUpgrade : IUpgradable
    {
        public bool HasBought => hasBought;
        
        public event Action OnBuy;
        public event Func<int, bool> OnCanBuy;
        
        private IWorldService world;
        private bool hasBought;
        private UpgradeValuesInspector values;
        private GameObject button;

        public void Setup(Vector3 offset, IWorldService world)
        {
            this.world = world;

            button = Utils.SpawnPrefab(values.buttonPrefab);
            button.transform.position += offset;

            world.Add(button, this);
        }

        public void SetValues(UpgradeValuesInspector values) => this.values = values;

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
            if (hasBought)
                return;

            if (!OnCanBuy.Invoke(values.cost))
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
        public event Func<int, bool> OnCanBuy;

        private UpgradeValuesInspector values;
        private GameObject button;

        public void SetValues(UpgradeValuesInspector values) => this.values = values;
        public void Setup(Vector3 offset, IWorldService world)
        {
            button = Utils.SpawnPrefab(values.buttonPrefab);
            button.transform.position += offset;

            world.Add(button, this);
        }


        public string GetHoverTitle()
        {
            if (!OnCanBuy.Invoke(values.cost))
                return "Can't afford yet!";

            return $"{values.name} for ${values.cost}";
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