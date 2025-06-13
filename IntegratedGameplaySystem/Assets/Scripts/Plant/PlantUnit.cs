using System.Collections.Generic;
using UnityEngine;
using System;

namespace IntegratedGameplaySystem
{
    public class PlantUnit : IStartable, IInteractable, IHoverable, IDisposable, IHarvestable, IWaterable
    {
        public const float LOWERED_COLLIDER_HEIGHT = -0.5f;
        
        public int Progress { get; set; }
        public bool IsWatered { get; set; }

        public readonly GameObject gameObject;
        public readonly Transform transform;
        public readonly PlantFlyweight flyweight;

        private readonly MeshRenderer[] meshRenderers;
        private readonly SphereCollider sphereCollider;
        private readonly IPoolService<PoolableParticle> pool;

        private readonly Dictionary<Type, PlantState> states = new();
        private MeshRenderer soilRend;
        private bool prevIsWatered;
        private PoolableParticle rainParticles;
        private PlantState currentState;

        public PlantUnit(PlantFlyweight flyweight)
        {
            this.flyweight = flyweight;

            gameObject = Utils.SpawnPrefab(flyweight.plantPrefab);
            transform = gameObject.transform;
            transform.name = flyweight.name;

            pool = ServiceLocator<IPoolService<PoolableParticle>>.Locate();
            sphereCollider = gameObject.AddComponent<SphereCollider>();
            sphereCollider.radius = 0.6f;
            meshRenderers = transform.GetComponentsInChildren<MeshRenderer>();
            SetupStates();
        }

        private void SetupStates()
        {
            states.Add(typeof(SoilState), new SoilState());
            states.Add(typeof(GrowingState), new GrowingState());
            states.Add(typeof(HarvestableState), new HarvestableState());

            foreach (var state in states)
            {
                state.Value.PassPlant(this);
            }

            currentState = states[typeof(SoilState)];
        }

        public void Start()
        {
            ServiceLocator<IWorldService>.Locate().Add(transform, this);
            EventManager.AddListener(Occasion.Tick, Tick);

            MakeSoilGraphics();

            RefreshMaterials();
            SetWatered(IsWatered);
            RefreshRainEffect();
            SetColliderHeight(LOWERED_COLLIDER_HEIGHT);

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
                currentState = states[typeof(HarvestableState)];
                SetColliderHeight(0);
            }
        }

        public void RefreshMaterials() 
        {
            bool visible = currentState.GetType() != typeof(SoilState);

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

            bool disabled = currentState.GetType() == typeof(GrowingState) && IsWatered;
            sphereCollider.enabled = !disabled;

            RefreshRainEffect();
        }
        
        public void SetColliderHeight(float y) 
        {
            sphereCollider.center = Vector3.up * y;
        }
    }
}