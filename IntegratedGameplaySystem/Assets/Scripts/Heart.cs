using System.Collections.Generic;
using UnityEngine;

namespace IntegratedGameplaySystem
{
    public class Heart : MonoBehaviour
    {
        public const int MAX_FPS = 300;
        public const float INTERVAL = 64f;

        [SerializeField] private bool locked = default;
        [SerializeField] private BaseBehaviour[] sceneBehaviours = default;

        private readonly List<BaseBehaviour> subscribers = new List<BaseBehaviour>();

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
            Time.fixedDeltaTime = 1f / INTERVAL;
            Application.targetFrameRate = MAX_FPS;

            Cursor.visible = !locked;
            Cursor.lockState = locked ? CursorLockMode.Locked : CursorLockMode.None;
        }

        private void Update() => subscribers.ForEach(x => x.Update());
        private void FixedUpdate() => subscribers.ForEach(x => x.FixedUpdate());
        private void OnApplicationQuit() => EventManager.RaiseEvent(Occasion.CLOSE_GAME);
    }
}