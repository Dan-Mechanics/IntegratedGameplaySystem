using System;
using UnityEngine;

namespace IntegratedGameplaySystem
{
    /// <summary>
    /// Could we technically consider this subclass sandbox? i dont think so.
    /// Maybe research subclass sandbox again ?
    /// Should prolly make interfaces...
    /// </summary>
    public abstract class BaseBehaviour : ScriptableObject
    {
        //public Action<BaseBehaviour> OnSubscribeRequest;
        public GameObject prefab;
        //[Min(1)] public int count = 1;

        protected Transform transform;
        protected GameObject gameObject;

        public void Setup(GameObject gameObject) 
        {
            this.gameObject = gameObject;
            transform = gameObject.transform;
        }

        protected Transform GetChild(int index, string expected)
        {
            if (index < 0)
            {
                Debug.LogError("invalid index");
                return null;
            }

            if (transform.childCount <= 0)
            {
                Debug.LogError("if(trans.childCount <= 0) ");
                return null;
            }

            if (index >= transform.childCount)
            {
                Debug.LogError("if (index >= trans.childCount) ");
                return null;
            }

            Transform child = transform.GetChild(index);
            if (child.name != expected)
            {
                Debug.LogError("if (child.name != expected) ");
                return null;
            }

            Debug.Log($"found {expected}.");
            return child;
        }

        public virtual void Start() { }
        public virtual void Disable() { }
        public virtual void Update() { }
        public virtual void FixedUpdate() { }
        public virtual void LateFixedUpdate() { }
    }
}