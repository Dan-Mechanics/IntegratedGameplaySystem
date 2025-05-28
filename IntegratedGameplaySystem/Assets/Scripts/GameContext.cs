using System.Collections.Generic;
using UnityEngine;

namespace IntegratedGameplaySystem
{
    [System.Serializable]
    public class GameContext : IStartable, IUpdatable, IDisposable
    {
        [SerializeField] private SceneSetup sceneSetup = default;
        [SerializeField] private List<GameObject> scenePrefabs = default;
        [SerializeField] private Assets assets = default;

        /// <summary>
        /// FIXME:
        /// what happens when shit needs to be deleted? this breaks basically.
        /// </summary>
        private readonly List<IUpdatable> updatables = new();
        private readonly List<IFixedUpdatable> fixedUpdatables = new();
        private readonly List<ILateFixedUpdatable> lateFixedUpdatables = new();
        private readonly List<IDisposable> disposables = new();

        private float timer;

        /// <summary>
        /// You could feed the components in here but i think it makes sense here.
        /// Or just make this heart or something.
        /// </summary>
        public void Start()
        {
            scenePrefabs.ForEach(x => Utils.SpawnPrefab(x));
            ServiceLocator<IAssetService>.Provide(assets);

            sceneSetup.Start();
            InputHandler inputHandler = InitializeInput(assets.GetBindingsConfig());
            ServiceLocator<IWorldService>.Provide(new GameWorld());

            List<object> components = new List<object>()
            {
                inputHandler,
                new Interactor(),
                new EasyDebug(),
                new PlayerContext()
                // gotta add plants.
            };

            List<IStartable> startables = new();
            foreach (object component in components)
            {
                SortComponent(component, startables);
                SortComponent(component, updatables);
                SortComponent(component, fixedUpdatables);
                SortComponent(component, lateFixedUpdatables);
                SortComponent(component, disposables);
            }

            startables.ForEach(x => x.Start());
        }

        private InputHandler InitializeInput(BindingsConfig bindingsConfig)
        {
            InputHandler inputHandler = new InputHandler(new DefaultBindingRules());
            List<Binding> bindings = bindingsConfig.GetBindings();
            bindings.ForEach(x => inputHandler.AddBinding(x));
            ServiceLocator<IInputService>.Provide(inputHandler);

            return inputHandler;
        }

        private void SortComponent<T>(object component, List<T> list)
        {
            if (component is T t)
                list.Add(t);
        }

        /// <summary>
        /// https://docs.unity3d.com/6000.0/Documentation/ScriptReference/Physics.Simulate.html
        /// </summary>
        public void Update()
        {
            updatables.ForEach(x => x.Update());

            timer += Time.deltaTime;
            while (timer >= Time.fixedDeltaTime)
            {
                timer -= Time.fixedDeltaTime;

                fixedUpdatables.ForEach(x => x.FixedUpdate());
                Physics.Simulate(Time.fixedDeltaTime);
                lateFixedUpdatables.ForEach(x => x.LateFixedUpdate());
            }
        }

        /// <summary>
        /// THIS MUST WORK FOR WHEN SWITCHING SCENES !! TEST THAT !!
        /// </summary>
        public void Dispose() => disposables.ForEach(x => x.Dispose());
    }
}