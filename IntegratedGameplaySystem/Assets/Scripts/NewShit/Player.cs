using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace IntegratedGameplaySystem
{
    public class Player : IStartable, IUpdatable
    {
        private SceneObject sceneObject;

        public Player()
        {
            sceneObject = new SceneObject(nameof(Player));
        }

        public void Start()
        {
            // ... 
        }

        public void Update()
        {
            // ... 
        }
    }
}