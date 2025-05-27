using UnityEngine;

namespace IntegratedGameplaySystem
{
    [System.Serializable]
    public class Binding
    {
        public string key;
        public PlayerAction playerAction;
        [HideInInspector] public KeyCode keyCode;

        public Binding(PlayerAction playerAction, KeyCode keyCode)
        {
            this.playerAction = playerAction;
            this.keyCode = keyCode;
        }
    }
}