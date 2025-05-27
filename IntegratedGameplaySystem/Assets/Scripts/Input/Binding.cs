using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace IntegratedGameplaySystem
{
    [Serializable]
    public class Binding
    {
        public string key;
        public PlayerAction playerAction;
        [HideInInspector] public KeyCode keyCode;

        public Binding(KeyCode keyCode, PlayerAction playerAction)
        {
            this.keyCode = keyCode;
            this.playerAction = playerAction;
        }
    }
}