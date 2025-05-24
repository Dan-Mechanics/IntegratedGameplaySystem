using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace IntegratedGameplaySystem
{
    public class Test 
    {
        protected Transform transform;
        protected GameObject gameObject;

        public Test Setup(GameObject prefab)
        {
            GameObject go = Object.Instantiate(prefab, prefab.transform.position, prefab.transform.rotation);
            go.name = prefab.name;

            gameObject = go;
            transform = gameObject.transform;

            return this;
        }

        public virtual void Start() { }
        public virtual void Disable() { }
        public virtual void Update() { }
        public virtual void FixedUpdate() { }
        public virtual void LateFixedUpdate() { }
    }
}