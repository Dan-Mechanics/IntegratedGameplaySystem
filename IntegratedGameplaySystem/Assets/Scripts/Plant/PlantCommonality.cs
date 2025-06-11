using System.Collections.Generic;
using UnityEngine;
using System;

namespace IntegratedGameplaySystem
{
    public class PlantCommonality : IStartable, IInteractable, IHoverable, IDisposable, IHarvestable, IWaterable
    {
        public int Progress { get; set; }
        public bool IsWatered { get; set; }

        public readonly GameObject gameObject;
        public readonly Transform transform;
        public readonly PlantFlyweight flyweight;

        private readonly MeshRenderer[] meshRenderers;
        private readonly SphereCollider sphereCollider;
        private readonly IPoolService<PoolableParticle> pool;

        private readonly Dictionary<Type, IPlantState> states = new();
        private MeshRenderer soilRend;
        private bool prevIsWatered;
        private PoolableParticle rainParticles;
        private IPlantState currentState;

        public PlantCommonality(PlantFlyweight flyweight)
        {
            this.flyweight = flyweight;

            gameObject = Utils.SpawnPrefab(flyweight.plantPrefab);
            transform = gameObject.transform;
            transform.name = flyweight.name;

            pool = ServiceLocator<IPoolService<PoolableParticle>>.Locate();
            sphereCollider = gameObject.AddComponent<SphereCollider>();
            sphereCollider.radius = 0.6f;
            meshRenderers = transform.GetComponentsInChildren<MeshRenderer>();

            states.Add(typeof(Soil), new Soil());
            states.Add(typeof(Growing), new Growing());
            states.Add(typeof(Harvestable), new Harvestable());

            foreach (var item in states)
            {
                item.Value.Plant = this;
            }

            currentState = states[typeof(Soil)];
        }

        public void Start()
        {
            ServiceLocator<IWorldService>.Locate().Add(transform, this);
            EventManager.AddListener(Occasion.Tick, Tick);

            MakeSoilGraphics();

            RefreshMaterials();
            SetWatered(IsWatered);
            RefreshRainEffect();
            SetColliderHeight(-0.5f);

            Interact();
        }

        private void MakeSoilGraphics()
        {
            Transform soil = GameObject.CreatePrimitive(PrimitiveType.Quad).transform;
            soil.name = "soil";
            soil.position = transform.position;
            soil.position += Vector3.down * 0.49f;
            soil.rotation = Quaternion.Euler(90f, 0f, 0f);
            UnityEngine.Object.Destroy(soil.GetComponent<Collider>());
            soilRend = soil.GetComponent<MeshRenderer>();
            soilRend.material = flyweight.drySoil;
            soilRend.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;

            soil.gameObject.isStatic = true;
        }

        public void Dispose()
        {
            EventManager.RemoveListener(Occasion.Tick, Tick);
        }

        public void Grow()
        {
            SetWatered(false);
            Progress++;
            Progress = Mathf.Clamp(Progress, 0, flyweight.materials.Length - 1);

            RefreshMaterials();

            if (Progress >= flyweight.materials.Length - 1)
            {
                currentState = states[typeof(Harvestable)];
                SetColliderHeight(0);
            }
        }

        public void RefreshMaterials() 
        {
            bool visible = currentState.GetType() != typeof(Soil);

            for (int i = 0; i < meshRenderers.Length; i++)
            {
                meshRenderers[i].enabled = visible;
                meshRenderers[i].material = flyweight.materials[Progress];
            }
        }

        public void RefreshRainEffect()
        {
            if (prevIsWatered == IsWatered)
                return;

            if (IsWatered)
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

            prevIsWatered = IsWatered;
        }
        
        public void Tick() => currentState.Tick();
        public void Interact() => currentState.Interact();
        public string GetHoverTitle() => currentState.GetHoverTitle();
        public void Harvest() => currentState.Harvest();
        public void Water() => currentState.Water();
        public void SetState(Type type) => currentState = states[type];
        
        public void SetWatered(bool value)
        {
            IsWatered = value;

            bool disabled = currentState.GetType() == typeof(Growing) && IsWatered;
            sphereCollider.enabled = !disabled;

            RefreshRainEffect();
        }
        
        public void SetColliderHeight(float y) 
        {
            sphereCollider.center = Vector3.up * y;
        }
    }

    public interface IPlantState : IInteractable, IHoverable, IWaterable, IHarvestable
    {
        PlantCommonality Plant { get; set; }
        void Tick();
    }

    public class Soil : IPlantState
    {
        public PlantCommonality Plant { get; set; }

        public string GetHoverTitle()
        {
            return $"barren {Plant.flyweight.name} soil";
        }

        public void Harvest() { }

        public void Tick() { }

        public void Interact()
        {
            Plant.SetState(typeof(Growing));
            Plant.RefreshMaterials();
        }

        public void Water() 
        {
            Plant.SetWatered(true);
        }
    }

    public class Growing : IPlantState
    {
        public PlantCommonality Plant { get; set; }

        public string GetHoverTitle()
        {
            if (!Plant.IsWatered)
                return $"dry {Plant.flyweight.name}";

            return Plant.flyweight.name;
        }

        public void Tick() 
        {
            if (!Utils.RandomWithPercentage(Plant.IsWatered ? Plant.flyweight.wateredGrowGrowPercentage : Plant.flyweight.dryGrowPercentage))
                return;

            Plant.Grow();
        }

        public void Harvest() { }

        public void Interact()
        {
            Water();
        }

        public void Water()
        {
            Plant.SetWatered(true);
        }
    }

    public class Harvestable : IPlantState
    {
        public PlantCommonality Plant { get; set; }

        public string GetHoverTitle()
        {
            return Plant.flyweight.name;
        }

        public void Harvest() 
        {
            Plant.Progress = 0;
            EventManager<IItemArchitype>.RaiseEvent(Occasion.PickupItem, Plant.flyweight);

            //Unit.RefreshCollider(false, false);
            Plant.SetColliderHeight(-0.5f);

            Plant.SetState(typeof(Soil));
            Plant.RefreshMaterials();
            Plant.SetWatered(false);
        }

        public void Interact()
        {
            Harvest();
        }

        public void Tick() { }

        public void Water()
        {
            Plant.SetWatered(true);
        }
    }
}