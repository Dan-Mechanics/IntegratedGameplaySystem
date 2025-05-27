using UnityEngine;

namespace IntegratedGameplaySystem
{
    public class Binding
    {
        public PlayerAction playerAction;
        public KeyCode key;

        public Binding(PlayerAction playerAction, KeyCode keyCode)
        {
            this.playerAction = playerAction;
            this.key = keyCode;
        }
    }
}