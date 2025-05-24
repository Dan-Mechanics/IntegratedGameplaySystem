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

        /*protected Transform GetChild(int index, string expected) 
        {
            if(index < 0)
            {
                Debug.LogError("invalid index");
                return null;
            }

            if(trans.childCount <= 0) 
            {
                Debug.LogError("if(trans.childCount <= 0) ");
                return null;
            }

            if (index >= trans.childCount) 
            {
                Debug.LogError("if (index >= trans.childCount) ");
                return null;
            }

            Transform child = trans.GetChild(index);
            if (child.name != expected) 
            {
                Debug.LogError("if (child.name != expected) ");
                return null;
            }

            Debug.Log($"found {expected}.");
            return child;
        }*/

        public virtual void Start() { }
        public virtual void Update() { }
        public virtual void FixedUpdate() { }
        public virtual void LateFixedUpdate() { }
    }
}