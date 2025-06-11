using System.Collections.Generic;
using UnityEngine;
using System;

namespace IntegratedGameplaySystem
{
    public class Plant : IStartable, IInteractable, IHoverable, IDisposable, IHarvestable, IWaterable
    {
        public int Progression { get; set; }
        public bool IsWatered { get; set; }

        public readonly GameObject gameObject;
        public readonly Transform transform;
        public readonly PlantFlyweight flyweight;

        private readonly MeshRenderer[] meshRenderers;
        private readonly SphereCollider sphereCollider;
        private readonly IPoolService<PoolableParticle> pool;

        private MeshRenderer soilRend;
        private bool prevIsWatered;
        private PoolableParticle rainParticles;

        private IPlantStage stage;
        private readonly Dictionary<Type, IPlantStage> stages = new();

        public Plant(PlantFlyweight flyweight)
        {
            this.flyweight = flyweight;

            gameObject = Utils.SpawnPrefab(flyweight.plantPrefab);
            transform = gameObject.transform;
            transform.name = flyweight.name;

            pool = ServiceLocator<IPoolService<PoolableParticle>>.Locate();
            sphereCollider = gameObject.AddComponent<SphereCollider>();
            sphereCollider.radius = 0.6f;
            meshRenderers = transform.GetComponentsInChildren<MeshRenderer>();

            stages.Add(typeof(Soil), new Soil());
            stages.Add(typeof(Growing), new Growing());
            stages.Add(typeof(Harvestable), new Harvestable());

            foreach (var item in stages)
            {
                item.Value.Plant = this;
            }

            stage = stages[typeof(Soil)];
        }

        public string GetHoverTitle() 
        {
            return stage.GetHoverTitle();
            
            /*if (!isPlanted)
                return $"barren {flyweight.name} soil";

            if (!isWatered && !IsHarvestable)
                return $"dry {flyweight.name}";

            return flyweight.name;*/
        }

        public void Start()
        {
            ServiceLocator<IWorldService>.Locate().Add(gameObject, this);
            EventManager.AddListener(Occasion.Tick, Tick);
            MakeSoilGraphics();

            RefreshMaterials();
            SetWatered(IsWatered);
            RefreshRainEffect();

            // make CONST for this PLEASS!!
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

        public void Tick()
        {
            stage.Tick();
        }

        public void Grow()
        {
            /*if (IsHarvestable)
                return;*/

            SetWatered(false);
            Progression++;
            Progression = Mathf.Clamp(Progression, 0, flyweight.materials.Length - 1);

            RefreshMaterials();
            //RefreshCollider(true, false);
            //RefreshRainEffect();

            if (Progression >= flyweight.materials.Length - 1)
            {
                stage = stages[typeof(Harvestable)];
                SetColliderHeight(0);
            }
        }

        public void RefreshMaterials() 
        {
            for (int i = 0; i < meshRenderers.Length; i++)
            {
                meshRenderers[i].enabled = typeof(Soil) != stage.GetType();
                meshRenderers[i].material = flyweight.materials[Progression];
            }
        }

        /*public void RefreshCollider(bool interactable, bool upPos) 
        {
            sphereCollider.enabled = interactable;
            sphereCollider.center = Vector3.down * (upPos ? 0.5f : 0f);
        }*/

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

        public void Interact()
        {
            /*if (!isPlanted) 
            {
                isPlanted = true;
                EventManager.AddListener(Occasion.Tick, Tick);
                RefreshMaterials();
                return;
            }
            
            Water();
            Harvest();*/

            stage.Interact();
        }

        public void Harvest()
        {
            stage.Harvest();
            
            /*if (!IsHarvestable)
                return;

            progression = 0;
            isWatered = false;
            isPlanted = false;
            EventManager.RemoveListener(Occasion.Tick, Tick);
            EventManager<IItemArchitype>.RaiseEvent(Occasion.PickupItem, flyweight);

            RefreshMaterials();
            RefreshCollider();
            RefreshRainEffect();*/
        }

        public void SetWatered(bool value)
        {
            /*if (isWatered == value)
                return;*/

            IsWatered = value;

            bool disabled = stage.GetType() == typeof(Growing) && IsWatered;

            sphereCollider.enabled = !disabled;
            RefreshRainEffect();
            //RefreshCollider(false, false);
        }
        
        public void SetColliderHeight(float h) 
        {
            sphereCollider.center = Vector3.up * h;
        }

        public void Water() => stage.Water();

        public void GoToStage(Type type) 
        {
            stage = stages[type];
        }
    }

    public interface IPlantStage : IInteractable, IHoverable, IWaterable, IHarvestable
    {
        Plant Plant { get; set; }
        void Tick();
    }

    public class Soil : IPlantStage
    {
        public Plant Plant { get; set; }

        public string GetHoverTitle()
        {
            return $"barren {Plant.flyweight.name} soil";
        }

        public void Harvest() { }

        public void Tick() { }

        public void Interact()
        {
            Plant.GoToStage(typeof(Growing));
            Plant.RefreshMaterials();
        }

        public void Water() 
        {
            Plant.SetWatered(true);
        }
    }

    public class Growing : IPlantStage
    {
        public Plant Plant { get; set; }

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

    public class Harvestable : IPlantStage
    {
        public Plant Plant { get; set; }

        public string GetHoverTitle()
        {
            return Plant.flyweight.name;
        }

        public void Harvest() 
        {
            Plant.Progression = 0;
            EventManager<IItemArchitype>.RaiseEvent(Occasion.PickupItem, Plant.flyweight);

            //Unit.RefreshCollider(false, false);
            Plant.SetColliderHeight(-0.5f);

            Plant.GoToStage(typeof(Soil));
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