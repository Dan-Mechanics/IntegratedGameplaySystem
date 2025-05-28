using System.Collections.Generic;
using UnityEngine;

namespace IntegratedGameplaySystem
{
    [System.Serializable]
    public class GameContext : IStartable, IUpdatable, IDisposable
    {
        public SceneSetup sceneSetup;
        public List<GameObject> scenePrefabs;
        public AssetScratchpad assetScratchpad;

        /// <summary>
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
            assetScratchpad.Start();
            ServiceLocator<IAssetService>.Provide(assetScratchpad);

            scenePrefabs.ForEach(x => Utils.SpawnPrefab(x));

            // INPUT ==========

            // nooit een ontkenning in je code, altijd is iets en niet not iets.
            // noem het exception.
            IBindingRule[] rules = { new DisallowMultiblePlayerAction() };
            InputHandler inputHandler = new InputHandler(rules);
            List<Binding> bindings = assetScratchpad.FindAsset<BindingsConfig>("config").GetBindings();

            //return;

            bindings.ForEach(x => inputHandler.AddBinding(x));
            
            // We provide the InputHandler after we create it.
            // I think that is correct but I am not certain.
            ServiceLocator<IInputService>.Provide(inputHandler);
            ServiceLocator<IWorldService>.Provide(new GameWorld());

            // FILTER THIS ??

            // functie geeft de leest mij in stoppen en er uit hae


            // input T , list is pass by refernece, en dan de list vna de dingen.
            // The order of this is important.
            List<object> components = new List<object>()
            {
                sceneSetup,
                inputHandler,
                new Interactor(),
                new EasyDebug(),
                //new PlayerContext()
                // gotta add plants.
            };

            List<IStartable> startables = new();
            foreach (object component in components)
            {
                // TE ZEGGEN FILTER.
                if (component is IStartable startable)
                    startables.Add(startable);

                if (component is IUpdatable updatable)
                    updatables.Add(updatable);

                if (component is IFixedUpdatable fixedUpdatable)
                    fixedUpdatables.Add(fixedUpdatable);

                if (component is ILateFixedUpdatable lateFixedUpdatable)
                    lateFixedUpdatables.Add(lateFixedUpdatable);

                if (component is IDisposable disposable)
                    disposables.Add(disposable);
            }

            // we must provide all services before the shit.
            startables.ForEach(x => x.Start());
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