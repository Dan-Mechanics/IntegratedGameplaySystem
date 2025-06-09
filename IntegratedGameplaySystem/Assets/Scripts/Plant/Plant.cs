using UnityEngine;

namespace IntegratedGameplaySystem
{
    /// <summary>
    /// State machine here ?? That might screw performance ??
    /// 
    /// PlantUnit.cs ?? SoilUnit ?
    /// </summary>
    public class Plant : IStartable, IInteractable, IHoverable, IDisposable, IHarvestable, IWaterable
    {
        /// <summary>
        /// Yes, I know we are defining a hard rule here.
        /// </summary>
        public bool IsHarvestable => progression >= flyweight.materials.Length - 1 && isPlanted;
        public bool IsInteractable => IsHarvestable || !isWatered || !isPlanted;

        public readonly GameObject gameObject;
        public readonly Transform transform;

        private readonly PlantFlyweight flyweight;
        private readonly MeshRenderer[] meshRenderers;
        private readonly SphereCollider sphereCollider;
        private readonly IPoolService<PoolableParticle> pool;

        private MeshRenderer soilRend;
        private int progression;
        private bool isWatered;
        private bool isPlanted;
        private bool prevIsWatered;
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
            if (!isPlanted)
                return $"barren {flyweight.name} soil";

            if (!isWatered && !IsHarvestable)
                return $"dry {flyweight.name}";

            return flyweight.name;
        }

        public void Start()
        {
            ServiceLocator<IWorldService>.Locate().Add(gameObject, this);
            //EventManager.AddListener(Occasion.Tick, Tick);
            MakeSoil();

            RefreshMaterials();
            RefreshCollider();
            RefreshRainEffect();
        }

        private void MakeSoil()
        {
            Transform soil = GameObject.CreatePrimitive(PrimitiveType.Quad).transform;
            soil.position = transform.position;
            soil.position += Vector3.down * 0.49f;
            soil.rotation = Quaternion.Euler(90f, 0f, 0f);
            Object.Destroy(soil.GetComponent<Collider>());
            soilRend = soil.GetComponent<MeshRenderer>();
            soilRend.material = flyweight.drySoil;
            soilRend.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;

            soil.gameObject.isStatic = true;
        }

        public void Dispose()
        {
            if (isPlanted)
                EventManager.RemoveListener(Occasion.Tick, Tick);
        }

        private void Tick()
        {
            // You could also just unsub here ??
            /*if (!isPlanted)
                return;*/
            
            if (!Utils.RandomWithPercentage(isWatered ? flyweight.wateredGrowGrowPercentage : flyweight.dryGrowPercentage))
                return;

            Grow();
        }

        private void Grow()
        {
            if (IsHarvestable)
                return;
            
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
                meshRenderers[i].enabled = isPlanted;
                meshRenderers[i].material = flyweight.materials[progression];
            }
        }

        private void RefreshCollider() 
        {
            sphereCollider.enabled = IsInteractable;
            sphereCollider.center = Vector3.down * (IsHarvestable ? 0f : 0.5f);
        }

        public void RefreshRainEffect()
        {
            if (prevIsWatered == isWatered)
                return;

            if (isWatered)
            {
                rainParticles = pool.Get();
                rainParticles.Place(transform.position + Vector3.up);
                soilRend.material = flyweight.wetSoil;
            }
            else
            {
                pool.Give(rainParticles);
                soilRend.material = flyweight.drySoil;
            }

            prevIsWatered = isWatered;
        }

        public void Interact()
        {
            if (!isPlanted) 
            {
                isPlanted = true;
                EventManager.AddListener(Occasion.Tick, Tick);
                RefreshMaterials();
                return;
            }
            
            Water();
            Harvest();
        }

        public void Harvest()
        {
            if (!IsHarvestable)
                return;

            progression = 0;
            isWatered = false;
            isPlanted = false;
            EventManager.RemoveListener(Occasion.Tick, Tick);
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