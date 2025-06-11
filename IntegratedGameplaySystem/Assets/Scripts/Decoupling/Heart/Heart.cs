using System.Collections.Generic;
using UnityEngine;

namespace IntegratedGameplaySystem
{
    /// <summary>
    /// Sends Start(), Update(), Dispose(), etc pulses to all the script in the game that need those.
    /// </summary>
    public class Heart
    {
        private readonly List<IUpdatable> updatables = new();
        private readonly List<IFixedUpdatable> fixedUpdatables = new();
        private readonly List<ILateFixedUpdatable> lateFixedUpdatables = new();
        private readonly List<IDisposable> disposables = new();

        private float timer;

        public void Setup(List<object> components)
        {
            List<IStartable> startables = new();
            foreach (object component in components)
            {
                Sort(component, startables);
                Sort(component, updatables);
                Sort(component, fixedUpdatables);
                Sort(component, lateFixedUpdatables);
                Sort(component, disposables);

                if (component is IDestroyable destroyable)
                    destroyable.OnDestroy += DestroyComponent;
            }

            startables.ForEach(x => x.Start());
            startables.Clear();
        }

        private void DestroyComponent(object component, GameObject go)
        {
            Remove(component, updatables);
            Remove(component, fixedUpdatables);
            Remove(component, lateFixedUpdatables);

            if (component is IDisposable disposable)
            {
                disposable.Dispose();
                disposables.Remove(disposable);
            }

            if (component is IDestroyable destroyable)
                destroyable.OnDestroy -= DestroyComponent;

            ServiceLocator<IWorldService>.Locate().Remove(go.transform);
        }

        private void Sort<T>(object component, List<T> list)
        {
            if (component is T t)
                list.Add(t);
        }

        private void Remove<T>(object component, List<T> list)
        {
            if (component is T t)
                list.Remove(t);
        }

        /// <summary>
        /// https://docs.unity3d.com/6000.0/Documentation/ScriptReference/Physics.Simulate.html
        /// </summary>
        public void Update()
        {
            for (int i = updatables.Count - 1; i >= 0; i--)
                updatables[i].Update();

            timer += Time.deltaTime;
            while (timer >= Time.fixedDeltaTime)
            {
                timer -= Time.fixedDeltaTime;

                for (int i = fixedUpdatables.Count - 1; i >= 0; i--)
                    fixedUpdatables[i].FixedUpdate();

                Physics.Simulate(Time.fixedDeltaTime);

                for (int i = lateFixedUpdatables.Count - 1; i >= 0; i--)
                    lateFixedUpdatables[i].LateFixedUpdate();
            }
        }

        /// <summary>
        /// This is so that scene switching doesn't create "ghost memory"
        /// because of statics.
        /// </summary>
        public void Dispose() 
        {
            for (int i = disposables.Count - 1; i >= 0; i--)
            {
                disposables[i].Dispose();
            }

            disposables.Clear();
            updatables.Clear();
            fixedUpdatables.Clear();
            lateFixedUpdatables.Clear();
        }
    }
}