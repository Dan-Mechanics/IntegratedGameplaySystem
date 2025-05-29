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

        public Binding(string keyString, PlayerAction playerAction)
        {
            this.keyString = keyString;
            this.playerAction = playerAction;
            ProcessKeyString();
        }

        public void ProcessKeyString() => keyCode = Utils.StringToEnum<KeyCode>(keyString);
        public string Log() => $"{keyString} --> {keyCode} | {playerAction}";
    }
}