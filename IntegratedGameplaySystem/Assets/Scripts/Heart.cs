using System;
using System.Collections.Generic;
using UnityEngine;

namespace IntegratedGameplaySystem
{
    public class Heart
    {
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
        public void Start(object[] components)
        {
            List<IStartable> startables = new();
            foreach (object component in components)
            {
                SortGameElement(component, startables);
                SortGameElement(component, updatables);
                SortGameElement(component, fixedUpdatables);
                SortGameElement(component, lateFixedUpdatables);
                SortGameElement(component, disposables);

                if (component is IDestroyable destroyable)
                    destroyable.OnDestroy += DestroyComponent;
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
        public void DestroyComponent(object component)
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