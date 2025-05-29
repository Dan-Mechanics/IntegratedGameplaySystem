using UnityEngine;
using System;

namespace IntegratedGameplaySystem
{
    [Serializable]
    public class Binding
    {
        public PlayerAction playerAction;
        public string keyString;
        [HideInInspector] public KeyCode keyCode;

        public Binding(PlayerAction playerAction, KeyCode keyCode)
        {
            this.playerAction = playerAction;
            this.keyCode = keyCode;
        }

        public void ProcessKeyString() => keyCode = Utils.StringToEnum<KeyCode>(keyString);
        public string Log() => $"{keyString} {playerAction} {keyCode}";
    }
}