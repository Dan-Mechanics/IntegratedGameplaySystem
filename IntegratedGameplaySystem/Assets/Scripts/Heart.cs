using System.Collections.Generic;
using UnityEngine;

namespace IntegratedGameplaySystem
{
    public class Heart : MonoBehaviour
    {
        [SerializeField] private BaseBehaviour[] behaviours = default;
        private readonly List<BaseBehaviour> subscribers = new List<BaseBehaviour>();
        
        private void Awake()
        {
            for (int i = 0; i < behaviours.Length; i++)
            {
                Register(behaviours[i]);
            }
        }

        /// <summary>
        /// Something like this idk ??
        /// </summary>
        private void Register(BaseBehaviour behaviour) 
        {
            GameObject go = Instantiate(behaviour.prefab, behaviour.prefab.transform.position, behaviour.prefab.transform.rotation);
            behaviour.Setup(go);
            subscribers.Add(behaviour);
        }

        private void Start()
        {
            subscribers.ForEach(x => x.Start());
        }

        private void Update()
        {
            subscribers.ForEach(x => x.Update());
        }

        private void FixedUpdate()
        {
            subscribers.ForEach(x => x.FixedUpdate());
        }
    }
}