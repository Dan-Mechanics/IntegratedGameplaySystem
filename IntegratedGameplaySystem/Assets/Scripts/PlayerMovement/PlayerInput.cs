using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace IntegratedGameplaySystem
{
    /// <summary>
    /// In the future this should be with InputHandler but ok.
    /// Make it more general to give the string names.
    /// It would be cool if i had aaaaaaaaaaaaaaaaa config system POGGG
    /// that might also be fun #noburnout divas.
    /// 
    /// TEMP FILE !!
    /// </summary>
    public class PlayerInput
    {
        private readonly InputBehaviour inputBehaviour;

        private bool forward;
        private bool back;
        private bool left;
        private bool right;

        public PlayerInput(InputBehaviour inputBehaviour)
        {
            this.inputBehaviour = inputBehaviour;

            inputBehaviour.GetAction(PlayerAction.Forward).OnChange += OnForward;
            inputBehaviour.GetAction(PlayerAction.Backward).OnChange += OnBack;
            inputBehaviour.GetAction(PlayerAction.Left).OnChange += OnLeft;
            inputBehaviour.GetAction(PlayerAction.Right).OnChange += OnRight;
        }

        private void OnForward(bool value) => forward = value;
        private void OnBack(bool value) => back = value;
        private void OnLeft(bool value) => left = value;
        private void OnRight(bool value) => right = value;

        public float Vertical() 
        {
            float z = 0f;

            if (forward)
                z++;

            if (back)
                z--;

            return z;
        }

        public float Horizontal()
        {
            float x = 0f;

            if (right)
                x++;

            if (left)
                x--;

            return x;
        }

        public Vector2 GetMouseInput() 
        {
            return new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
        }

        public void Dispose() 
        {
            inputBehaviour.GetAction(PlayerAction.Forward).OnChange -= OnForward;
            inputBehaviour.GetAction(PlayerAction.Backward).OnChange -= OnBack;
            inputBehaviour.GetAction(PlayerAction.Left).OnChange -= OnLeft;
            inputBehaviour.GetAction(PlayerAction.Right).OnChange -= OnRight;
        }
    }
}