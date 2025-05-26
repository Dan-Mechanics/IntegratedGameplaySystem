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
        [SerializeField] private List<BaseBehaviour> scene = default;

        private readonly List<BaseBehaviour> subscribers = new();
        private float timer;
        
        private void Start()
        {
            Setup();

            for (int i = 0; i < scene.Count; i++)
            {
                Register(scene[i]);
            }

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
            Physics.simulationMode = SimulationMode.Script;
            Time.fixedDeltaTime = 1f / INTERVAL;
            Application.targetFrameRate = MAX_FPS;

            Cursor.visible = !locked;
            Cursor.lockState = locked ? CursorLockMode.Locked : CursorLockMode.None;
        }

        /// <summary>
        /// https://docs.unity3d.com/6000.0/Documentation/ScriptReference/Physics.Simulate.html
        /// </summary>
        private void Update() 
        {
            subscribers.ForEach(x => x.Update());

            timer += Time.deltaTime;
            while (timer >= Time.fixedDeltaTime)
            {
                timer -= Time.fixedDeltaTime;

                subscribers.ForEach(x => x.FixedUpdate());
                Physics.Simulate(Time.fixedDeltaTime);
                subscribers.ForEach(x => x.LateFixedUpdate());
            }
        }

        private void OnDisable() => subscribers.ForEach(x => x.Disable());

        private void OnApplicationQuit() => EventManager.RaiseEvent(Occasion.CLOSE_GAME);
    }
}