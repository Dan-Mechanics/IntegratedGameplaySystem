using System.Collections.Generic;
using UnityEngine;

namespace IntegratedGameplaySystem
{
    [System.Serializable]
    public class GameContext : IStartable, IUpdatable, IDisposable
    {
        public SceneSetup sceneSetup;
        public List<GameObject> scenePrefabs;

        private float timer;
        private readonly List<IUpdatable> updatables = new();
        private readonly List<IFixedUpdatable> fixedUpdatables = new();
        private readonly List<ILateFixedUpdatable> lateFixedUpdatables = new();
        private readonly List<IDisposable> disposables = new();

        /// <summary>
        /// You could feed the components in here but i think it makes sense here.
        /// Or just make this heart or something.
        /// </summary>
        public void Start()
        {
            List<IStartable> startables = new();

            // welcome to c# !!
            InputHandler inputHandler = new InputHandler(new InputHandler.IBindingRule[] { new InputHandler.DisallowMultiblePlayerAction() });

            ServiceLocator<IInputService>.Provide(inputHandler);
            ServiceLocator<IWorldService>.Provide(new GameWorld());

            // the order of this is importnat.
            List<object> components = new List<object>()
            {
                sceneSetup,
                inputHandler,
                new Interactor(),
                new EasyDebug(),
                new PlayerContext()
            };

            foreach (object component in components)
            {
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