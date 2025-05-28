using System;
using System.Collections.Generic;
using UnityEngine;

namespace IntegratedGameplaySystem
{
    [System.Serializable]
    public class GameContext : IStartable, IUpdatable
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

            List<object> gameElements = new List<object>()
            {
                inputHandler,
                new Interactor(),
                new EasyDebug(),
                new PlayerContext()
                // gotta add plants.
            };

            // I dont want to be able to start things after the game has started
            // with this. the host class should call a constructor or a setup or start method
            // on the nwely created thing. the newly created thigns are owned by the maker.
            List<IStartable> startables = new();
            foreach (object gameElement in gameElements)
            {
                SortGameElement(gameElement, startables);
                SortGameElement(gameElement, updatables);
                SortGameElement(gameElement, fixedUpdatables);
                SortGameElement(gameElement, lateFixedUpdatables);
                
                if(gameElement is IDisposable disposable)
                {
                    disposables.Add(disposable);
                    disposable.OnDispose += DisposeSomething;
                }
            }

            startables.ForEach(x => x.Start());
            startables.Clear();
        }

        private InputHandler InitializeInput(BindingsConfig bindingsConfig)
        {
            InputHandler inputHandler = new InputHandler(new DefaultBindingRules());
            List<Binding> bindings = bindingsConfig.GetBindings();
            bindings.ForEach(x => inputHandler.AddBinding(x));
            ServiceLocator<IInputService>.Provide(inputHandler);

            return inputHandler;
        }

        /// <summary>
        /// or then maybe make it work via something else idk ??
        /// I think this works fine for now...
        /// It isnt really needed anyway.
        /// 
        /// Ok, this code is getting bad now, im getting tired.
        /// I tihnk i need to make a seperate interface for disposing events and destroying things, like i desteroyable. that makes more sense.
        /// but they live in the same wheelhouse right ?? whatever. look at this in the morning.
        /// </summary>
        public void DisposeSomething(object gameElement) 
        {
            Remove(gameElement, updatables);
            Remove(gameElement, fixedUpdatables);
            Remove(gameElement, lateFixedUpdatables);

            if (gameElement is IDisposable disposable)
                disposable.OnDispose -= DisposeSomething;
        }

        private void SortGameElement<T>(object gameElement, List<T> list)
        {
            if (gameElement is T t)
                list.Add(t);
        }

        private void Remove<T>(object gameElement, List<T> list)
        {
            if (gameElement is T t)
                list.Remove(t);
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