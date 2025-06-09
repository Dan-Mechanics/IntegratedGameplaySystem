using UnityEngine;

namespace IntegratedGameplaySystem
{
    public class Plant : IStartable, IInteractable, IHoverable, IDisposable, IHarvestable, IWaterable
    {
        /// <summary>
        /// Yes, I know we are defining a hard rule here.
        /// </summary>
        public bool IsHarvestable => progression >= flyweight.materials.Length - 1;

        public readonly GameObject gameObject;
        public readonly Transform transform;

        private readonly PlantFlyweight flyweight;
        private readonly MeshRenderer[] meshRenderers;
        private readonly SphereCollider sphereCollider;
        private readonly IPoolService<PoolableParticle> pool;

        private int progression;
        private bool isWatered;
        private bool rainEffectShowing;
        private PoolableParticle rainParticles;

        public Plant(PlantFlyweight flyweight)
        {
            this.flyweight = flyweight;

            gameObject = Utils.SpawnPrefab(flyweight.plantPrefab);
            transform = gameObject.transform;
            transform.name = flyweight.name;

            pool = ServiceLocator<IPoolService<PoolableParticle>>.Locate();
            sphereCollider = gameObject.AddComponent<SphereCollider>();
            meshRenderers = transform.GetComponentsInChildren<MeshRenderer>();
        }

        public string GetHoverTitle() 
        {
            if (!isWatered && !IsHarvestable)
                return $"dry {flyweight.name}";

            return flyweight.name;
        }

        public void Start()
        {
            ServiceLocator<IWorldService>.Locate().Add(gameObject, this);
            EventManager.AddListener(Occasion.Tick, Tick);

            RefreshMaterials();
            RefreshCollider();
            RefreshRainEffect();
        }

        public void Dispose()
        {
            EventManager.RemoveListener(Occasion.Tick, Tick);
        }

        private void Tick()
        {
            if (!Utils.RandomWithPercentage(isWatered ? flyweight.wateredGrowGrowPercentage : flyweight.dryGrowPercentage))
                return;

            Grow();
        }

        private void Grow()
        {
            isWatered = false;
            progression++;
            progression = Mathf.Clamp(progression, 0, flyweight.materials.Length - 1);

            RefreshMaterials();
            RefreshCollider();
            RefreshRainEffect();
        }

        private void RefreshMaterials() 
        {
            for (int i = 0; i < meshRenderers.Length; i++)
            {
                meshRenderers[i].material = flyweight.materials[progression];
            }
        }

        private void RefreshCollider() 
        {
            sphereCollider.center = Vector3.down * (IsHarvestable ? 0f : 0.5f);
            sphereCollider.enabled = IsHarvestable || !isWatered;
        }

        public void RefreshRainEffect()
        {
            if (rainEffectShowing == isWatered)
                return;

            if (isWatered)
            {
                rainParticles = pool.Get();
                rainParticles.Place(transform.position + Vector3.up);
            }
            else
            {
                pool.Give(rainParticles);
            }

            rainEffectShowing = isWatered;
        }

        public void Interact()
        {
            Water();
            Harvest();
        }

        public void Harvest()
        {
            if (!IsHarvestable)
                return;

            if (flyweight.name != "Feathercup")
                Debug.LogWarning($"{flyweight.name} harvested !");
            else
                Debug.Log($"{flyweight.name} harvested !");

            progression = 0;
            isWatered = false;
            EventManager<IItemArchitype>.RaiseEvent(Occasion.PickupItem, flyweight);

            RefreshMaterials();
            RefreshCollider();
            RefreshRainEffect();
        }

        public void Water()
        {
            if (isWatered)
                return;

            isWatered = true;

            RefreshRainEffect();
            RefreshCollider();
        }
    }
}