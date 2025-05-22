using UnityEngine;

namespace IntegratedGameplaySystem
{
    public abstract class BaseBehaviour : ScriptableObject
    {
        public GameObject prefab;

        /// <summary>
        /// These are for fun.
        /// </summary>
        protected Vector3 pos => trans.position;
        /// <summary>
        /// These are for fun.
        /// </summary>
        protected Quaternion rot => trans.rotation;

        protected Transform trans;
        protected GameObject go;

        public void Setup(GameObject go) 
        {
            this.go = go;
            trans = go.transform;
        }

        protected void print(string msg) => Debug.Log(msg);
        protected T Fetch<T>() => trans.GetComponent<T>();

        public virtual void Start() { }
        public virtual void Update() { }
        public virtual void FixedUpdate() { }
    }
}