using System.Collections.Generic;
using UnityEngine;
using System;

namespace IntegratedGameplaySystem
{
    /// <summary>
    /// State machine here ?? That might screw performance ??
    /// 
    /// PlantUnit.cs ?? SoilUnit ?
    /// 
    /// Does this class need to be improved further because we have quite a lot of bool bullshit
    /// going on here.
    /// IDKkkkk i dont feel like making another 100000 classsi for this dumb shit.
    /// </summary>
    public class SoilUnit : IStartable, IInteractable, IHoverable, IDisposable, IHarvestable, IWaterable
    {
        /// <summary>
        /// Yes, I know we are defining a hard rule here.
        /// </summary>
        //public bool IsHarvestable => progression >= flyweight.materials.Length - 1 && isPlanted;
      //  public bool IsInteractable => IsHarvestable || !isWatered || !isPlanted;

        public readonly GameObject gameObject;
        public readonly Transform transform;

        public readonly PlantFlyweight flyweight;
        private readonly MeshRenderer[] meshRenderers;
        private readonly SphereCollider sphereCollider;
        private readonly IPoolService<PoolableParticle> pool;

        private MeshRenderer soilRend;
        public int progression;
        public bool isWatered;
        //private bool isPlanted;
        private bool prevIsWatered;
        private PoolableParticle rainParticles;

        private IPlantStage stage;
        private readonly Dictionary<Type, IPlantStage> stages = new();

        public SoilUnit(PlantFlyweight flyweight)
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
            SetWatered(isWatered);
            RefreshRainEffect();

            // make CONST for this PLEASS!!
            SetHeight(-0.5f);

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
            progression++;
            progression = Mathf.Clamp(progression, 0, flyweight.materials.Length - 1);

            RefreshMaterials();
            //RefreshCollider(true, false);
            RefreshRainEffect();

            if (progression >= flyweight.materials.Length - 1)
            {
                stage = stages[typeof(Harvestable)];
                SetHeight(0);
            }
        }

        public void RefreshMaterials() 
        {
            for (int i = 0; i < meshRenderers.Length; i++)
            {
                meshRenderers[i].enabled = typeof(Soil) != stage.GetType();
                meshRenderers[i].material = flyweight.materials[progression];
            }
        }

        /*public void RefreshCollider(bool interactable, bool upPos) 
        {
            sphereCollider.enabled = interactable;
            sphereCollider.center = Vector3.down * (upPos ? 0.5f : 0f);
        }*/

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
            if (isWatered == value)
                return;

            isWatered = value;
            sphereCollider.enabled = !value;
            RefreshRainEffect();
            //RefreshCollider(false, false);
        }

        public void SetHeight(float h) 
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
        SoilUnit Unit { get; set; }
        void Tick();
    }

    public class Soil : IPlantStage
    {
        public SoilUnit Unit { get; set; }

        public string GetHoverTitle()
        {
            return $"barren {Unit.flyweight.name} soil";
        }

        public void Harvest() { }

        public void Tick() { }

        public void Interact()
        {
            Unit.RefreshMaterials();
            Unit.GoToStage(typeof(Growing));
        }

        public void Water() 
        {
            Unit.SetWatered(true);
        }
    }

    public class Growing : IPlantStage
    {
        public SoilUnit Unit { get; set; }

        public string GetHoverTitle()
        {
            if (!Unit.isWatered)
                return $"dry {Unit.flyweight.name}";

            return Unit.flyweight.name;
        }

        public void Tick() 
        {
            if (!Utils.RandomWithPercentage(Unit.isWatered ? Unit.flyweight.wateredGrowGrowPercentage : Unit.flyweight.dryGrowPercentage))
                return;

            Unit.Grow();
        }

        public void Harvest() { }

        public void Interact()
        {
            Water();
        }

        public void Water()
        {
            Unit.SetWatered(true);
        }
    }

    public class Harvestable : IPlantStage
    {
        public SoilUnit Unit { get; set; }

        public string GetHoverTitle()
        {
            return $"barren {Unit.flyweight.name} soil";
        }

        public void Harvest() 
        {
            Unit.progression = 0;
            Unit.SetWatered(false);
            EventManager<IItemArchitype>.RaiseEvent(Occasion.PickupItem, Unit.flyweight);

            Unit.RefreshMaterials();
            //Unit.RefreshCollider(false, false);
            Unit.SetHeight(-0.5f);
            
            //Unit.RefreshRainEffect();
        }

        public void Interact()
        {
            Harvest();
        }

        public void Tick() { }

        public void Water()
        {
            Unit.SetWatered(true);
        }
    }
}