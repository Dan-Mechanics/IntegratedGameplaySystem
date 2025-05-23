using UnityEngine;

namespace IntegratedGameplaySystem
{
    /// <summary>
    /// Could we technically consider this subclass sandbox? i dont think so.
    /// Maybe research subclass sandbox again ?
    /// </summary>
    public abstract class BaseBehaviour : ScriptableObject
    {
        public GameObject prefab;

        protected Transform trans;
        protected GameObject go;

        public void Setup(GameObject go) 
        {
            this.go = go;
            trans = go.transform;
        }

        public virtual void Start() { }
        public virtual void Update() { }
        public virtual void FixedUpdate() { }
    }
}