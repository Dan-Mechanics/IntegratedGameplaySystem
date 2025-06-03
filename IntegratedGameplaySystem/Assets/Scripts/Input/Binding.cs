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

        public override bool Equals(object obj)
        {
            if (obj is not Binding other)
                return false;

            return playerAction == other.playerAction && keyCode == other.keyCode;
        }

        public void ProcessKeyString() => keyCode = Utils.StringToEnum<KeyCode>(keyString);
        public override int GetHashCode() => base.GetHashCode();
        public override string ToString() => $"{keyString} --> {keyCode} | {playerAction}";
    }
}