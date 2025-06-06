using System.Collections.Generic;
using UnityEngine;

namespace IntegratedGameplaySystem
{
    /// <summary>
    /// You could rename to scene context. because it spawns all the stuff and confiures it.
    /// Scene Assembler?
    /// </summary>
    public abstract class SceneBehaviour : ScriptableObject, IStartable, IDisposable
    {
        public NextSceneHandler sceneHandler;

        public virtual void Start() => sceneHandler.Start();
        public virtual List<object> GetSceneComponents() => new List<object>();
        public virtual void Dispose() => sceneHandler.Dispose();
    }
}