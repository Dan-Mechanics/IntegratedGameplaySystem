using System.Collections.Generic;
using UnityEngine;

namespace IntegratedGameplaySystem
{
    [System.Serializable]
    public class GameContext : IStartable, IUpdatable, IDisposable
    {
        [SerializeField] private string assetsPath = "assets";
        [SerializeField] private string sceneSetupPath = "scene_setup";

        [SerializeField] private List<GameObject> scenePrefabs = default;

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
            Resources.Load<SceneSetup>(sceneSetupPath).Start();

            AssetScratchpad scratchpad = InitializeAssets(assetsPath);
            InputHandler inputHandler = InitializeInput(scratchpad, "config");

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

        private AssetScratchpad InitializeAssets(string path)
        {
            AssetScratchpad scratchpad = Resources.Load<AssetScratchpad>(path);
            scratchpad.Start();
            ServiceLocator<IAssetService>.Provide(scratchpad);
            return scratchpad;
        }

        private InputHandler InitializeInput(AssetScratchpad scratchpad, string path)
        {
            InputHandler inputHandler = new InputHandler(new DefaultBindingRules());
            List<Binding> bindings = scratchpad.FindAsset<BindingsConfig>(path).GetBindings();
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