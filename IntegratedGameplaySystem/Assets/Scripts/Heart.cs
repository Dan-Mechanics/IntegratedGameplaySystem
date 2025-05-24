using System.Collections.Generic;
using UnityEngine;

namespace IntegratedGameplaySystem
{
    public class Heart : MonoBehaviour
    {
        /// <summary>
        /// YOu could outsource this but why would you ??
        /// </summary>
        public const int MAX_FPS = 300;
        public const float INTERVAL = 64f;

        [SerializeField] private bool locked = default;
        [SerializeField] private BaseBehaviour[] sceneBehaviours = default;

        private readonly List<BaseBehaviour> subscribers = new List<BaseBehaviour>();
        private readonly FixedTicks fixedTicks = new FixedTicks(1f / INTERVAL);
        private int count;

        private void Start()
        {
            for (int i = 0; i < sceneBehaviours.Length; i++)
            {
                Register(sceneBehaviours[i]);
            }

            Setup();
            subscribers.ForEach(x => x.Start());
        }

        private void Register(BaseBehaviour behaviour)
        {
            subscribers.Add(behaviour);

            if (behaviour.prefab == null)
                return;

            GameObject go = Instantiate(behaviour.prefab, behaviour.prefab.transform.position, behaviour.prefab.transform.rotation);
            go.name = behaviour.prefab.name;

            behaviour.Setup(go);
        }

        private void Setup()
        {
            Physics.autoSimulation = false;
            Time.fixedDeltaTime = 1f / INTERVAL;
            Application.targetFrameRate = MAX_FPS;

            Cursor.visible = !locked;
            Cursor.lockState = locked ? CursorLockMode.Locked : CursorLockMode.None;
        }

        private void Update() 
        {
            subscribers.ForEach(x => x.Update());
            count = fixedTicks.GetTicksCount(Time.deltaTime);

            for (int i = 0; i < count; i++)
            {
                subscribers.ForEach(x => x.FixedUpdate());
            }

            for (int i = 0; i < count; i++)
            {
                subscribers.ForEach(x => x.LateFixedUpdate());
            }
        }

        private void OnApplicationQuit() => EventManager.RaiseEvent(Occasion.CLOSE_GAME);
    }
}